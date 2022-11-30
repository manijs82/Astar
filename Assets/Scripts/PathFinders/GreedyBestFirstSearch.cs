using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedyBestFirstSearch : BreadthFirstSearch
{
    
    private Dictionary<PathGridObject, float> frontiersPriorityQueue;
    
    protected override IEnumerator PathFindingSearch()
    {
        frontiersPriorityQueue = new Dictionary<PathGridObject, float>();
        frontiersPriorityQueue.Add(pathGrid.start, 0);
        searched = new Dictionary<PathGridObject, PathGridObject>();
        searched.Add(pathGrid.start, pathGrid.start);

        while (frontiersPriorityQueue.Count > 0)
        {
            yield return new WaitForSeconds(.03f);
            PathGridObject current = GetHighestPriority();
            frontiersPriorityQueue.Remove(current);
            if(current == pathGrid.goal)
                break;
            
            foreach (var gridObj in pathGrid.GetNeighbors(current))
            {
                
                // yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.D));
                if (!searched.ContainsKey(gridObj))
                {
                    frontiersPriorityQueue.Add(gridObj, GetDist(pathGrid.goal.Point, gridObj.Point));
                    searched.Add(gridObj, current);
                    gridObj.ToggleHighLight(true);
                    gridObj.SetArrowDirection(pathGrid.GetDirectionFromGridObjects(gridObj, current));
                }
            }
        }

        HighlightPath();
    }

    private PathGridObject GetHighestPriority()
    {
        PathGridObject o = null;
        float lowestKey = 10000;
        foreach (var gridObject in frontiersPriorityQueue)
        {
            if (gridObject.Value < lowestKey)
            {
                lowestKey = gridObject.Value;
                o = gridObject.Key;
            }
        }

        return o;
    }

    private float GetDist(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }
}