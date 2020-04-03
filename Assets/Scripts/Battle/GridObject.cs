﻿using System;
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
    public GridUnit HoldedUnit => _holdedUnit;

    public GridElement(Vector2Int id, Vector3 worldPos)
    {
        _id = id;
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
    [SerializeField] private GridElement[,] _objects = new GridElement[10, 10];
    [SerializeField] private float _tileOffset = 1;
    [SerializeField] private GameObject _ElementTemplate;
    [SerializeField] private UnitsData _data;
    [SerializeField] private List<GridUnit> _unitsOnGrid = new List<GridUnit>();

    public enum ElementState
    {
        normal = 0,
        vacant = 1,
        locked = 2
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

    public void PredictPlace(GridUnit unit, ElementState state)
    {
        List<GridElement> vacantElements = GetVacantElements(unit, 0);

        UpdateGridEngagements();

        bool locked = false;

        foreach(var element in vacantElements)
        {
            if(element.HoldedUnit != unit && element.HoldedUnit != null)
            {
                locked = true;
                break;
            }
        }
        
        SetElementsState(vacantElements, locked == true ? ElementState.locked : ElementState.vacant);
    }

    private List<GridElement> GetVacantElements(GridUnit unit, int offset)
    {
        Vector3Int unitPos = Vector3Int.RoundToInt(unit.transform.position);
        List<GridElement> vacantElements = new List<GridElement>();

        var rotation = unit.GetDirection();

        switch (rotation)
        {
            case GridUnit.RotationDirection.Forward:
                for (int x = -offset; x < unit.Size.y + offset; x++)
                {
                    for (int z = -offset; z < unit.Size.x + offset; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(unitPos.x + x, 1, unitPos.z - z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }
                break;

            case GridUnit.RotationDirection.Right:
                for (int x = -offset; x < unit.Size.x + offset; x++)
                {
                    for (int z = -offset; z < unit.Size.y + offset; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(unitPos.x - x, 1, unitPos.z - z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }
                break;

            case GridUnit.RotationDirection.Back:

                for (int x = -offset; x < unit.Size.y + offset; x++)
                {
                    for (int z = -offset; z < unit.Size.x + offset; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(unitPos.x - x, 1, unitPos.z + z);

                        if (VectorInRange(vacantVector))
                        {
                            vacantElements.Add(_objects[vacantVector.x, vacantVector.z]);
                        }
                    }
                }

                break;
            case GridUnit.RotationDirection.Left:
                for (int x = -offset; x < unit.Size.x + offset; x++)
                {
                    for (int z = -offset; z < unit.Size.y + offset; z++)
                    {
                        Vector3Int vacantVector = new Vector3Int(unitPos.x + x, 1, unitPos.z + z);

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

    private void Start()
    {
        InitGrid();
    }

    public void UpdateGridEngagements()
    {
        foreach (GridElement element in _objects)
        {
            if(element.Engagement != 0)
                element.SetElementEngagement(ElementState.normal);
        }
    }

    public bool TryPlaceUnit(GridUnit unit)
    {
        UpdateGridEngagements();

        Vector3Int unitPos = Vector3Int.RoundToInt(unit.transform.position);
        List<GridElement> vacantElements = GetVacantElements(unit, 0);

        unit.transform.position = unitPos;

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
        List<GridElement> vacantElements = GetVacantElements(unit, 0);
        unit.SetElements(vacantElements);
        if(_unitsOnGrid.Contains(unit) == false)
            _unitsOnGrid.Add(unit);
    }

    public void RemoveUnit(GridUnit removableUnit)
    {
        if(_unitsOnGrid.Contains(removableUnit))
        {
            _unitsOnGrid.Remove(removableUnit);
            removableUnit.RemoveElements();
        }
    }

    private void SetElementsState(List<GridElement> elements, ElementState state)
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