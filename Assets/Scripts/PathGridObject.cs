using System;
using HyperCasual_Engine.Utils;
using TMPro;
using UnityEngine;

public class PathGridObject : MonoBehaviour
{
    [SerializeField] private Color obstacleColor;
    [SerializeField] private Color pathHighlightColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color normalColor;
    [SerializeField] private Color normalBodyColor;
    [SerializeField] private TextMeshPro costText;
    [SerializeField] private Transform arrow;
    
    [HideInInspector] public bool isWall;
    [HideInInspector] public int cost = 1;
    [HideInInspector] public int x, y;

    private bool highlighted;
    private Material mat;
    private static readonly int Color2 = Shader.PropertyToID("_Color2");
    private static readonly int Color1 = Shader.PropertyToID("_Color1");

    public Vector2 Point => new (x, y);
    public Vector2Int PointInt => new (x, y);

    public int Cost
    {
        get => cost;
        set
        {
            cost = value;
            costText.text = cost.ToString();
        }
    }

    public void Initialize(Grid<PathGridObject> pathGrid, int x, int y)
    {
        mat = GetComponentInChildren<Renderer>().material;
        this.x = x;
        this.y = y;

        transform.position = pathGrid.GetWorldPosition(x, y);
    }

    public void SetArrowDirection(Direction direction)
    {
        arrow.gameObject.SetActive(true);
        arrow.rotation = Quaternion.Euler(0,0, direction.GetAngle());
    }
    
    public void SetNormal()
    {
        isWall = false;
        mat.SetColor(Color2, normalColor);
        mat.SetColor(Color1, normalBodyColor);
        Cost = 1;
    }
    
    public void SetObstacle()
    {
        isWall = false;
        mat.SetColor(Color2, obstacleColor);
        mat.SetColor(Color1, obstacleColor);
        Cost = 20;
    }
    
    public void SetWall()
    {
        isWall = true;
        mat.SetColor(Color2, obstacleColor);
        mat.SetColor(Color1, obstacleColor);
        costText.text = String.Empty;
    }

    public void ToggleHighLight()
    {
        mat.SetColor(Color2, highlighted ? normalColor : highlightColor);
        highlighted = !highlighted;
    }
    
    public void ToggleHighLight(bool active)
    {
        mat.SetColor(Color2, active ? highlightColor : normalColor);
    }
    
    public void TogglePathHighLight()
    {
        mat.SetColor(Color2, pathHighlightColor);
        mat.SetColor(Color1, pathHighlightColor);
    }
}