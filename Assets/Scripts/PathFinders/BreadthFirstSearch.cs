using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathGrid))]
public class BreadthFirstSearch : MonoBehaviour
{
    protected PathGrid pathGrid;
    
    protected Queue<PathGridObject> frontiers;
    protected Dictionary<PathGridObject, PathGridObject> searched;
    protected List<PathGridObject> path;

    private void Start()
    {
        pathGrid = GetComponent<PathGrid>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
            Reset();
        if(Input.GetKeyDown(KeyCode.Space) && 
           pathGrid.start != null && pathGrid.goal != null)
            StartPathFinding();
    }
    
    protected virtual void Reset()
    {
        StopAllCoroutines();
        foreach (var gridObject in pathGrid.grid.GetAll())
        {
            gridObject.SetNormal();
            gridObject.ToggleHighLight(false);
        }
    }

    protected virtual void StartPathFinding()
    {
        StopAllCoroutines();
        foreach (var gridObject in pathGrid.grid.GetAll()) gridObject.ToggleHighLight(false);
        StartCoroutine(PathFindingSearch());
    }
    
    protected virtual IEnumerator PathFindingSearch()
    {
        frontiers = new Queue<PathGridObject>();
        frontiers.Enqueue(pathGrid.start);
        searched = new Dictionary<PathGridObject, PathGridObject>();
        searched.Add(pathGrid.start, default);

        while (frontiers.Count > 0)
        {
            PathGridObject current = frontiers.Dequeue();
            if(current == pathGrid.goal)
                break;

            foreach (var gridObj in pathGrid.GetNeighbors(current))
            {
                // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D));
                yield return null;
                if (!searched.ContainsKey(gridObj))
                {
                    frontiers.Enqueue(gridObj);
                    searched.Add(gridObj, current);
                    gridObj.ToggleHighLight(true);
                    gridObj.SetArrowDirection(pathGrid.GetDirectionFromGridObjects(gridObj, current));
                }
            }
        }

        HighlightPath();
    }

    protected void HighlightPath()
    {
        PathGridObject currentPlace = pathGrid.goal;
        path = new List<PathGridObject>();
        
        while (currentPlace != pathGrid.start)
        {
            path.Add(currentPlace);
            currentPlace = searched[currentPlace];
        }
        path.Add(pathGrid.start);

        foreach (var gridObj in path)
            gridObj.TogglePathHighLight();
    }
}