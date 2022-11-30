using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstrasAlgorithm : BreadthFirstSearch
{

    protected Dictionary<PathGridObject, float> frontiersPriorityQueue;
    protected Dictionary<PathGridObject, int> costs;
    
    protected override IEnumerator PathFindingSearch()
    {
        frontiersPriorityQueue = new Dictionary<PathGridObject, float>();
        frontiersPriorityQueue.Add(pathGrid.start, 0);
        searched = new Dictionary<PathGridObject, PathGridObject>();
        searched.Add(pathGrid.start, pathGrid.start);
        costs = new Dictionary<PathGridObject, int>();
        costs.Add(pathGrid.start, 0);

        while (frontiersPriorityQueue.Count > 0)
        {
            yield return new WaitForSeconds(.03f);
            PathGridObject current = GetHighestPriority();
            frontiersPriorityQueue.Remove(current);
            
            if(current == pathGrid.goal)
                break;
            
            foreach (var gridObj in pathGrid.GetNeighbors(current))
            {
                int nextCost = costs[current] + current.cost + gridObj.cost;
                if (!costs.ContainsKey(gridObj) || nextCost < costs[gridObj])
                {
                    if(costs.ContainsKey(gridObj))
                        costs[gridObj] = nextCost;
                    else    
                        costs.Add(gridObj, nextCost);
                    if(frontiersPriorityQueue.ContainsKey(gridObj))
                        frontiersPriorityQueue[gridObj] = GetPriority(nextCost, gridObj.Point);
                    else    
                        frontiersPriorityQueue.Add(gridObj, GetPriority(nextCost, gridObj.Point));
                    if(searched.ContainsKey(gridObj))
                        searched[gridObj] = current;
                    else    
                        searched.Add(gridObj, current);
                    gridObj.ToggleHighLight(true);
                }
            }
        }
        
        HighlightPath();
    }

    protected virtual float GetPriority(int costSoFar, Vector2 nextPos) => costSoFar;

    protected PathGridObject GetHighestPriority()
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
}