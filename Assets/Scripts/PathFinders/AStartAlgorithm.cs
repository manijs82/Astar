using UnityEngine;

public class AStartAlgorithm : DijkstrasAlgorithm
{
    protected override float GetPriority(int costSoFar, Vector2 nextPos)
    {
        return costSoFar + GetDist(pathGrid.goal.Point, nextPos);
    }
    
    private float GetDist(Vector2 a, Vector2 b)
    {
        return Vector2.Distance(a, b);
    }
}
    