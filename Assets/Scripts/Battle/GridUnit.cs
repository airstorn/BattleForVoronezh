using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridUnit : MonoBehaviour
{
    [SerializeField] protected GameObject _visual;
    [SerializeField] protected Vector2Int _size;

    public Action OnDrag;

    public int Rotation => _rotation;
    public Vector2Int Size => _size;

    private int _rotation;
    private HashSet<GridElement> _engagedElements = new HashSet<GridElement>();

    public enum RotationDirection
    {
        Forward = 90,
        Right = 180,
        Back = 270,
        Left = 0
    }

    public void Rotate()
    {
        _rotation += 90;
        if (_rotation >= 360)
            _rotation = 0;
    }

    public void SetEngagedElements(HashSet<GridElement> elements, Grid.ElementState state)
    {
        _engagedElements = elements;

        foreach (GridElement element in _engagedElements)
        {
            element.SetElementEngagement(state);
        }
    }

    public void RemoveEngagedElements()
    {
        foreach (GridElement element in _engagedElements)
        {
            element.SetElementEngagement(Grid.ElementState.normal);
            element.SetUnit(null);
        }
    }

    public RotationDirection GetDirection()
    {
        return (RotationDirection)Mathf.RoundToInt(_rotation);
    }
}
