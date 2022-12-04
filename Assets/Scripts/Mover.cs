using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private BreadthFirstSearch pathFinder;
    [SerializeField] private float speed;

    private List<PathGridObject> path;
    private int currentPlace;

    private void Start()
    {
        pathFinder.OnPathCalculated += MoveOnPath;
    }

    private void MoveOnPath(List<PathGridObject> path)
    {
        currentPlace = 0;
        path.RemoveAt(0);
        this.path = path;
        MoveToNext();
    }

    private void MoveToNext()
    {
        if (currentPlace == path.Count)
            return;
        transform.DOLocalMove(path[currentPlace].Point + new Vector2(0.5f, 0.5f), speed).onComplete += () =>
        {
            currentPlace++;
            MoveToNext();
        };
        
    }
}