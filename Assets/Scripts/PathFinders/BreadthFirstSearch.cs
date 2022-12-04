using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathGrid))]
public class BreadthFirstSearch : MonoBehaviour
{
    public event Action<List<PathGridObject>> OnPathCalculated;
    
    protected PathGrid pathGrid;
    
    protected Queue<PathGridObject> frontiers;
    protected Dictionary<PathGridObject, PathGridObject> searched;
    protected List<PathGridObject> path;

    private void Awake()
    {
        pathGrid = GetComponent<PathGrid>();
        pathGrid.pathFinder = this;
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

    private void StartPathFinding()
    {
        StopAllCoroutines();
        foreach (var gridObject in pathGrid.grid.GetAll()) gridObject.ToggleHighLight(false);
        PathFindingSearch();
    }
    
    protected virtual void PathFindingSearch()
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
                //yield return null;
                if (!searched.ContainsKey(gridObj))
                {
                    frontiers.Enqueue(gridObj);
                    searched.Add(gridObj, current);
                    ChangeTileVisuals(gridObj, current);
                }
            }
        }

        HighlightPath();
    }

    protected void ChangeTileVisuals(PathGridObject gridObj, PathGridObject current)
    {
        gridObj.ToggleHighLight(true);
        gridObj.SetArrowDirection(pathGrid.GetDirectionFromGridObjects(gridObj, current));
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
        
        path.Reverse();
        OnPathCalculated?.Invoke(path);
    }
}