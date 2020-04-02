using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GridElement
{
    [SerializeField] private Vector2Int _id;
    [SerializeField] private GridUnit _unit;
    [SerializeField] private Vector3 _cellPos;

    public Vector2Int Id => _id;
    public GridUnit Unit => _unit;
    public Vector3 CellPos => _cellPos;

    public GridElement(Vector2Int id, Vector3 worldPos, GridUnit unit)
    {
        _id = id;
        _unit = unit;
        _cellPos = worldPos;
    }
}

public class Grid : MonoBehaviour
{
    [SerializeField] private GridElement[,] _objects = new GridElement[10, 10];
    [SerializeField] private float _tileOffset = 1;
    [SerializeField] private GameObject _cellTemplate;
    [SerializeField] private UnitsData _data;

    private void Start()
    {
        InitGrid();
    }

    private void InitGrid()
    {
        for (int x = 0; x < _objects.GetLength(0); x++)
        {
            for (int z = 0; z < _objects.GetLength(1); z++)
            {
                _objects[x, z] = new GridElement(
                    new Vector2Int(x, z),
                    new Vector3(x * _tileOffset,0, z * _tileOffset),
                    _data.GetUnit<GridUnits.Empty>()
                    );

                var cell = Instantiate(_cellTemplate, _objects[x, z].CellPos, Quaternion.Euler(90,0,0));
            }
        }
    }
}
