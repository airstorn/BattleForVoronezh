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
    public bool SuitablePlaced = false;

    public int Rotation => _rotation;
    public Vector2Int Size => _size;
    public Vector3Int PositionId { get; set; }

    private int _rotation;
    private bool _hidden;

   [SerializeField] private List<GridElement> _engagedElements = new List<GridElement>();

    public enum RotationDirection
    {
        Forward = 90,
        Right = 180,
        Back = 270,
        Left = 0
    }

    public void SetHidden(bool hide)
    {
        _hidden = hide;
        _visual.SetActive(_hidden);
    }

    public void Rotate()
    {
        _rotation += 90;

        if (_rotation >= 360)
            _rotation = 0;

        transform.DORotate(new Vector3(0, Rotation, 0), 0.3f);
    }

    public void Rotate(RotationDirection direction)
    {
        _rotation = (int)direction;
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

    public RotationDirection GetDirection()
    {
        return (RotationDirection)_rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.9f);

        Vector3 posOffset = new Vector3(_visual.transform.position.x, transform.position.y, _visual.transform.position.z);

        Gizmos.DrawCube(posOffset, GetDirection() == RotationDirection.Left || GetDirection() == RotationDirection.Right ?
            new Vector3(Size.x, 1, Size.y) :
            new Vector3(Size.y, 1, Size.x)
            );


        Gizmos.color = new Color(1, 0, 0, 0.9f);
        Gizmos.DrawWireCube(posOffset, new Vector3(Size.x + 2, 1, Size.y + 2));
    }
}
