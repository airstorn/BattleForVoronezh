using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridElement
{
    [SerializeField] private Vector2Int _id;
    [SerializeField] private Vector3 _ElementPos;
    [SerializeField] private SpriteRenderer _cellRenderer;
    [SerializeField] private GridUnit _holdedUnit;

    public int Engagement = 0;
    public SpriteRenderer CellRenderer => _cellRenderer;
    public Vector2Int Id => _id;
    public Vector3 CellPos => _ElementPos;


    public GridElement(Vector2Int id, Vector3 worldPos)
    {
        _id = id;
        _ElementPos = worldPos;
    }

    public void SetUnit(GridUnit unit)
    {
        _holdedUnit = unit;
    }
    
    public void SetElementEngagement(Grid.ElementState state)
    {
        switch (state)
        {
            case Grid.ElementState.locked:
                _cellRenderer.color = Color.red;
                break;
            case Grid.ElementState.normal:
                _cellRenderer.color = Color.white;
                break;
            case Grid.ElementState.vacant:
                _cellRenderer.color = Color.green;
                break;
        }

        Engagement = (int)state;
    }

    public void SetElement(SpriteRenderer rend)
    {
        _cellRenderer = rend;
    }
}

[Serializable]
public class ElementColors
{
    public Color Normal;
    public Color Locked;
    public Color Warning;
}

public class Grid : MonoBehaviour
{
    [SerializeField] private GridElement[,] _objects = new GridElement[10, 10];
    [SerializeField] private float _tileOffset = 1;
    [SerializeField] private GameObject _ElementTemplate;
    [SerializeField] private UnitsData _data;
    [SerializeField] private GridUnit[] _unitsOnGrid;

    [SerializeField] private ElementColors _elementColors;

    public enum ElementState
    {
        normal = 0,
        vacant = 1,
        locked = 2
    }

    public void PredictPlace(GridUnit currentUnit)
    {
        Vector3Int unitPos = Vector3Int.RoundToInt(currentUnit.transform.position);
        HashSet<GridElement> vacantElements = new HashSet<GridElement>();

        for (int x = 0; x < currentUnit.Size.x; x++)
        {
            for (int z = 0; z < currentUnit.Size.y; z++)
            {
                Vector3Int vacantVector = new Vector3Int(unitPos.x + x, 1, unitPos.z + z);

                if (VectorInRange(vacantVector))
                {
                    vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                }
            }
        }
        currentUnit.SetEngagedElements(vacantElements, ElementState.vacant);
    }

    private void Start()
    {
        InitGrid();
    }

    public void UpdateGridEngagements()
    {
        foreach (GridElement element in _objects)
        {
            if(element.Engagement == 1)
                element.SetElementEngagement(ElementState.normal);
        }
    }

    private void InitGrid()
    {
        for (int x = 0; x < _objects.GetLength(0); x++)
        {
            for (int z = 0; z < _objects.GetLength(1); z++)
            {
                _objects[x, z] = new GridElement(
                    new Vector2Int(x, z),
                    new Vector3(
                        transform.position.x - 4.5f + x * _tileOffset,
                        1, 
                        transform.position.z - 4.5f + z * _tileOffset)
                    );

                var element = Instantiate(_ElementTemplate, _objects[x, z].CellPos, Quaternion.Euler(90,0,0), transform);

                _objects[x, z].SetElement(element.GetComponent<SpriteRenderer>());
            }
        }
    }

    public bool TryPlaceUnit(GridUnit unit)
    {
        Debug.Log(unit);
        var direction = unit.GetDirection();

        Vector3Int unitPos = Vector3Int.RoundToInt(unit.transform.position);
        HashSet<GridElement> vacantElements = new HashSet<GridElement>();

        unit.transform.position = unitPos;

        for (int x = 0; x < unit.Size.x; x++)
        {
            for (int z = 0; z < unit.Size.y; z++)
            {
                Vector3Int vacantVector = new Vector3Int((int)unit.transform.position.x + x, 1, (int)unit.transform.position.z + z);

                if (VectorInRange(vacantVector))
                {
                    vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                    _objects[vacantVector.x, vacantVector.z].SetUnit(unit);
                }
                else
                {
                    return false;
                }
            }
        }

        unit.SetEngagedElements(vacantElements, ElementState.normal);

        return true;
    }

    private bool VectorInRange(Vector3Int unitPos)
    {
        if (unitPos.x < _objects.GetLength(0) && unitPos.x >= 0 && unitPos.z < _objects.GetLength(1) && unitPos.z >= 0)
            return true;
        else
            return false;
    }
}
