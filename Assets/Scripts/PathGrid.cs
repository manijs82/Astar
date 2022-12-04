using System.Collections.Generic;
using HyperCasual_Engine.Utils;
using UnityEngine;

public class PathGrid : MonoBehaviour
{
    [SerializeField] private PathGridObject gridObject;
    [SerializeField] protected Transform startTf;
    [SerializeField] protected Transform goalTf;

    [HideInInspector] public BreadthFirstSearch pathFinder;
    [HideInInspector] public PathGridObject start;
    [HideInInspector] public PathGridObject goal;
    public Grid<PathGridObject> grid;
    private bool neighborAlter;

    private void Awake()
    {
        grid = new Grid<PathGridObject>(20, 20, 1, CreateGridObject,
            new Vector3(transform.position.x, transform.position.y));
    }

    private void Start()
    {
        pathFinder.OnPathCalculated += list => start = list[^1];
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            var gridObj = GetGridObjAtMouse();
            gridObj?.SetObstacle();
            return;
        }

        if (Input.GetKey(KeyCode.S))
        {
            var gridObj = GetGridObjAtMouse();
            gridObj?.SetNormal();
            return;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            var gridObj = GetGridObjAtMouse();
            gridObj?.SetWall();
            return;
        }

        if (Input.GetMouseButtonDown(0))
            SetGridObjectToMousePos(ref start, startTf);

        if (Input.GetMouseButtonDown(1))
        {
            SetGridObjectToMousePos(ref goal, goalTf);
        }
    }

    private PathGridObject GetGridObjAtMouse()
    {
        var clickedPoint = MouseUtils.GetMouseClickedPoint(Camera.main);
        var gridObj = grid.GetValue(clickedPoint);
        return gridObj;
    }

    private void SetGridObjectToMousePos(ref PathGridObject gridObj, Transform tf)
    {
        var go = GetGridObjAtMouse();
        if (go == null) return;
        gridObj = go;
        tf.position = grid.GetWorldPosition(gridObj.x, gridObj.y) + new Vector3(0.5f, 0.5f);
    }

    private PathGridObject CreateGridObject(Grid<PathGridObject> pathGrid, int x, int y)
    {
        var gridObj = Instantiate(gridObject);
        gridObj.Initialize(pathGrid, x, y);

        return gridObj;
    }

    public List<PathGridObject> GetNeighbors(PathGridObject center)
    {
        var output = new List<PathGridObject>();
        var temp = new List<PathGridObject>();

        neighborAlter = !neighborAlter;

        if(neighborAlter)
        {
            temp.Add(grid.GetValue(center.x, center.y + 1));
            temp.Add(grid.GetValue(center.x, center.y - 1));
            temp.Add(grid.GetValue(center.x + 1, center.y));
            temp.Add(grid.GetValue(center.x - 1, center.y));
        }
        else
        {
            temp.Add(grid.GetValue(center.x, center.y - 1));
            temp.Add(grid.GetValue(center.x, center.y + 1));
            temp.Add(grid.GetValue(center.x - 1, center.y));
            temp.Add(grid.GetValue(center.x + 1, center.y));
        }

        foreach (var p in temp)
            if (p != null)
            {
                if (!p.isWall)
                    output.Add(p);
            }

        return output;
    }

    public Direction GetDirectionFromGridObjects(PathGridObject from, PathGridObject to)
    {
        var dir = to.PointInt - from.PointInt;
        return dir.GetDirection();
    }
}