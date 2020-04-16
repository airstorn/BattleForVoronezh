using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[Serializable]
public class UnitVisual
{
    [SerializeField] private GameObject _healthy;
    [SerializeField] private GameObject _broken;
    [SerializeField] private bool _hidden;

    public void SetBroken(bool state)
    {
        _broken.SetActive(state);

        if(_hidden == true)
            _healthy.SetActive(!state);
    }

    public void SetHidden(bool hidden)
    {
        _hidden = hidden;
        _healthy.SetActive(!_hidden);
    }
}

public class GridUnit : MonoBehaviour
{
    [SerializeField] private UnitVisual _visual;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private List<GridElement> _engagedElements = new List<GridElement>();
    [SerializeField] private List<GridElement> _borderElements = new List<GridElement>();
    [SerializeField] private Transform _gizmosPoint;

    public List<GridElement> Borders => _borderElements;
    public int Rotation => _rotation;
    public UnitVisual Visual => _visual;
    public Vector2Int Size => _size;
    public Vector3Int PositionId { get; set; }

    private int _rotation;
    public bool SuitablePlaced
    {
        get
        {
            if (_engagedElements.Count == _size.x * _size.y)
            {
                return true;
            }

            return false;
        }
    }

    public enum RotationDirection
    {
        Forward = 90,
        Right = 180,
        Back = 270,
        Left = 0
    }

    private void Awake()
    {
        _visual.SetBroken(false);
        SetHidden(false);
    }

    public void SetHidden(bool hide)
    {
        _visual.SetHidden(hide);
    }

    public void Rotate(bool imidietly)
    {
        _rotation += 90;

        if (_rotation >= 360)
            _rotation = 0;

        transform.DORotate(new Vector3(0, Rotation, 0), imidietly ? 0 : 0.3f);
    }

    public void Rotate(RotationDirection direction)
    {
        _rotation = (int)direction;
        transform.DORotate(new Vector3(0, Rotation, 0), 0.3f);
    }

    public void SetElements(IEnumerable<GridElement> selfElements, IEnumerable<GridElement> borderElements)
    {
        RemoveElements();

        foreach (var element in selfElements)
        {
            element.SetUnit(this);
            _engagedElements.Add(element);
        }

        _borderElements = borderElements.Except(_engagedElements).ToList();
    }


    public void RemoveElements()
    {
        foreach (var element in _engagedElements.Where(element => element.HoldedUnit == this))
        {
            element.SetUnit(null);
        }
        _borderElements.Clear();
        _engagedElements.Clear();
    }

    public RotationDirection GetDirection()
    {
        return (RotationDirection)_rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.9f);

        Vector3 posOffset = new Vector3(_gizmosPoint.position.x, transform.position.y, _gizmosPoint.position.z);
        Vector3 FantomOffset = new Vector3(PositionId.x + Size.x / 2, transform.position.y, PositionId.z + Size.y / 2);

        Gizmos.DrawCube(posOffset, GetDirection() == RotationDirection.Left || GetDirection() == RotationDirection.Right ?
            new Vector3(Size.x, 1, Size.y) :
            new Vector3(Size.y, 1, Size.x)
            );


        Gizmos.color = new Color(1, 0, 0, 0.9f);
        Gizmos.DrawWireCube(posOffset, GetDirection() == RotationDirection.Left || GetDirection() == RotationDirection.Right ?
           new Vector3(Size.x + 2, 1, Size.y + 2) :
           new Vector3(Size.y + 2, 1, Size.x + 2)
           );

    }
}
