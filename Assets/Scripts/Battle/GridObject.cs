using System;
using System.Collections.Generic;
using UnityEngine;
using static GridUnit;

[Serializable]
public class GridElement
{
    //[SerializeField] private Vector2Int _id;
    [SerializeField] private Vector3 _ElementPos;
    [SerializeField] private SpriteRenderer _cellRenderer;
    [SerializeField] private GridUnit _holdedUnit;

    public int Engagement = 0;
    public SpriteRenderer CellRenderer => _cellRenderer;
    //public Vector2Int Id => _id;
    public Vector3 CellPos => _ElementPos;
    public GridUnit HoldedUnit => _holdedUnit;

    public GridElement(Vector2Int id, Vector3 worldPos)
    {
        //_id = id;
        _ElementPos = worldPos;
    }

    public void SetUnit(GridUnit unit)
    {
        _holdedUnit = unit;
    }
    
    public void SetElementEngagement(GridObject.ElementState state)
    {
        switch (state)
        {
            case GridObject.ElementState.locked:
                _cellRenderer.color = Color.red;
                break;
            case GridObject.ElementState.normal:
                _cellRenderer.color = Color.white;
                break;
            case GridObject.ElementState.vacant:
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

public class GridObject : MonoBehaviour
{
    [SerializeField] private GridElement[,] _objects;
    [SerializeField] private float _tileOffset = 1;
    [SerializeField] private GameObject _ElementTemplate;
    [SerializeField] private UnitsData _data;
    [SerializeField] private List<GridUnit> _unitsOnGrid = new List<GridUnit>();

    [Tooltip("is units hidden by default on this grid?")]
    [SerializeField] private bool _hiddenUnits;
    [SerializeField] private Vector2Int _size;

    public Vector3Int GridOffset => _gridOffset;
    public GridElement[,] Sheet => _objects;
    public List<GridUnit> Units => _unitsOnGrid;

    private Vector3Int _gridOffset;

    public enum ElementState
    {
        normal = 0,
        vacant = 1,
        locked = 2
    }

    private void InitGrid()
    {
        _objects = new GridElement[_size.x, _size.y];

        for (int x = 0; x < _objects.GetLength(0); x++)
        {
            for (int z = 0; z < _objects.GetLength(1); z++)
            {
                _objects[x, z] = new GridElement(
                    new Vector2Int(x, z),
                    new Vector3(
                        transform.position.x - 4.5f + x * _tileOffset,
                        transform.position.y, 
                        transform.position.z - 4.5f + z * _tileOffset)
                    );

                var element = Instantiate(_ElementTemplate, _objects[x, z].CellPos, Quaternion.Euler(90,0,0), transform);

                _objects[x, z].SetElement(element.GetComponent<SpriteRenderer>());
            }
        }

        _gridOffset = Vector3Int.RoundToInt(_objects[0, 0].CellPos);
    }

    public List<GridElement> GetVacantElements(Vector3Int position, Vector2Int size, RotationDirection rotation, int borderOffsetRange)
    {
        List<GridElement> vacantElements = new List<GridElement>();

        switch (rotation)
        {
            case GridUnit.RotationDirection.Forward:
                for (int x = -borderOffsetRange; x < size.y + borderOffsetRange; x++)
                {
                    for (int z = -borderOffsetRange; z < size.x + borderOffsetRange; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(position.x + x - _gridOffset.x, 1, position.z - z - _gridOffset.z);
                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }
                break;

            case GridUnit.RotationDirection.Right:
                for (int x = -borderOffsetRange; x < size.x + borderOffsetRange; x++)
                {
                    for (int z = -borderOffsetRange; z < size.y + borderOffsetRange; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(position.x - x - _gridOffset.x, 1, position.z - z - _gridOffset.z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }
                break;

            case GridUnit.RotationDirection.Back:

                for (int x = -borderOffsetRange; x < size.y + borderOffsetRange; x++)
                {
                    for (int z = -borderOffsetRange; z < size.x + borderOffsetRange; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(position.x - x - _gridOffset.x, 1, position.z + z - _gridOffset.z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }

                break;
            case GridUnit.RotationDirection.Left:
                for (int x = -borderOffsetRange; x < size.x + borderOffsetRange; x++)
                {
                    for (int z = -borderOffsetRange; z < size.y + borderOffsetRange; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(position.x + x - _gridOffset.x, 1 , position.z + z - _gridOffset.z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }
                break;
        }
        return vacantElements;
    }

    public GridElement GetVacantElement(Vector3Int predictedPosition)
    {
        Vector3Int vacantVector = new Vector3Int(predictedPosition.x - _gridOffset.x, 1, predictedPosition.z - _gridOffset.z);
        if (VectorInRange(vacantVector))
        {
            return _objects[vacantVector.x, vacantVector.z];
        }

        throw new NotImplementedException("Element not found!");
    }

    private void Awake()
    {
        InitGrid();
    }

    public void UpdateGridEngagements()
    {
        foreach (GridElement element in _objects)
        {
                element.SetElementEngagement(ElementState.normal);
        }

        foreach(GridUnit unit in _unitsOnGrid)
        {
            List<GridElement> unitElements = GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 1);
            foreach (var currentElement in unitElements)
            {
                if (currentElement.HoldedUnit != null && currentElement.HoldedUnit != unit)
                {
                    SetElementsState(unitElements, ElementState.locked);
                    return;
                }
            }
        }
    }

    public bool AllUnitsPlaced()
    {
        return _unitsOnGrid.Count == _data.Data.Length;
    }

    public bool TryPlaceUnit(GridUnit unit)
    {
        UpdateGridEngagements();

        Vector3Int unitPos = Vector3Int.RoundToInt(unit.transform.position);
        List<GridElement> nearElements = GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 1);
        List<GridElement> vacantElements = GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 0);

        unit.transform.position = unitPos;

        foreach (var outlineElement in nearElements)
        {
            if(outlineElement.HoldedUnit != null && outlineElement.HoldedUnit != unit)
            {
                SetElementsState(vacantElements, ElementState.locked);
                return false;
            }
        }

        if (vacantElements.Count == (unit.Size.x * unit.Size.y))
        {
            foreach(var emptyElement in vacantElements)
            {
                if(emptyElement.HoldedUnit != null && emptyElement.HoldedUnit != unit)
                {
                    SetElementsState(vacantElements, ElementState.locked);
                    return false;
                }
            }
            return true;
        }
        else
        {
            SetElementsState(vacantElements, ElementState.locked);
            return false;
        }
    }

    public void PlaceUnit(GridUnit unit)
    {
        if (_unitsOnGrid.Contains(unit) == false)
        {
            _unitsOnGrid.Add(unit);
        }
        List<GridElement> vacantElements = GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 0);
        unit.SetElements(vacantElements);
        unit.SetHidden(!_hiddenUnits);

        if(vacantElements.Count > 0)
            unit.transform.position = vacantElements[0].CellPos;

        UpdateGridEngagements();
    }

    public void RemoveUnit(GridUnit removableUnit)
    {
        if(_unitsOnGrid.Contains(removableUnit))
        {
            removableUnit.RemoveElements();
            _unitsOnGrid.Remove(removableUnit);
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _unitsOnGrid.Count; i++)
        {
            _unitsOnGrid[i].RemoveElements();
        }
        _unitsOnGrid.Clear();
    }

    public void SetElementsState(List<GridElement> elements, ElementState state)
    {
        foreach (var item in elements)
        {
            item.SetElementEngagement(state);
        }
    }

    private bool VectorInRange(Vector3Int unitPos)
    {
        if (unitPos.x < _objects.GetLength(0) && unitPos.x >= 0 && unitPos.z < _objects.GetLength(1) && unitPos.z >= 0)
            return true;
        else
            return false;
    }
}
