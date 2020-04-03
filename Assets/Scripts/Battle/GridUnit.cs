using DG.Tweening;
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
   [SerializeField] private List<GridElement> _engagedElements = new List<GridElement>();

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

        transform.DORotate(new Vector3(0, Rotation, 0), 0.3f);
    }

    public void SetElements(List<GridElement> elements)
    {
        RemoveElements();

        foreach (var element in elements)
        {
            element.SetUnit(this);
            _engagedElements.Add(element);
        }
    }

    public void RemoveElements()
    {
        foreach (GridElement element in _engagedElements)
        {
            if (element.HoldedUnit == this)
            {
                element.SetUnit(null);
            }
        }
        _engagedElements.Clear();
    }

    public Vector2Int GetDirectionVector()
    {
        switch (_rotation)
        {
            case (int)RotationDirection.Forward:
                return new Vector2Int(1,-1);
            case (int)RotationDirection.Right:
                return new Vector2Int(-1, -1);
            case (int)RotationDirection.Back:
                return new Vector2Int(-1,1);
            case (int)RotationDirection.Left:
                return new Vector2Int(1,1);
            default:
                return Vector2Int.zero;
        }
    }

    public RotationDirection GetDirection()
    {
        return (RotationDirection)_rotation;
    }
}
