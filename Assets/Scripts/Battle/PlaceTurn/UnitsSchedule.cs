using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ScheduleObject
{
    public Vector3 Pos;
    [SerializeField] private GridUnit _holdedObject;
    private Tween _moveTween;

    public GridUnit HoldedObject
    {
        get
        {
            return _holdedObject;
        }
        set
        {
            _holdedObject = value;
            if (value != null)
               _moveTween = _holdedObject.transform.DOMove(Pos, 1);
        }
    }

    public void RemoveUnit()
    {
        _moveTween.Kill();
        _holdedObject = null;
    }

}


public class UnitsSchedule : MonoBehaviour, IUnitsData
{
    [SerializeField] private Transform _selectPoint;
    [SerializeField] private UnitsData _units;

    [SerializeField] private ScheduleObject[] _schedulePoints;

    private void Start()
    {
        SetUnits(_units);
    }

    public GridUnit GetUnit(GridUnit choosedUnit)
    {
        for (int i = 0; i < _schedulePoints.Length; i++)
        {
            if (_schedulePoints[i].HoldedObject == choosedUnit)
            {
                var choosedElement = _schedulePoints[i].HoldedObject;

                _schedulePoints[i].RemoveUnit();
                UpdateSchedule();
                return choosedElement;
            }
        }

        return null;
    }

    public void SetUnits(UnitsData data)
    {
        PoolSchedulePoints();

        for (int i = 0; i < _units.Data.Length; i++)
        {
            var createdObject = Instantiate(_units.Data[i].gameObject, _schedulePoints[i].Pos, Quaternion.identity);
            SetUnit(createdObject.GetComponent<GridUnit>());
        }
    }

    private void PoolSchedulePoints()
    {
        _schedulePoints = new ScheduleObject[_units.Data.Length];
        for (int i = 0; i < _schedulePoints.Length; i++)
        {
            _schedulePoints[i] = new ScheduleObject();
            _schedulePoints[i].Pos = new Vector3(_selectPoint.position.x + i * 5, _selectPoint.position.y, _selectPoint.position.z);
        }
    }

    private void UpdateSchedule()
    {
        for (int i = 0; i < _schedulePoints.Length - 1; i++)
        {
            if (_schedulePoints[i].HoldedObject == null && _schedulePoints[i + 1].HoldedObject != null)
            {
                SetUnit(_schedulePoints[i + 1].HoldedObject);
                _schedulePoints[i + 1].RemoveUnit();
            }
        }
    }

    public void SetUnit(GridUnit unit)
    {
        for (int i = 0; i < _schedulePoints.Length; i++)
        {
            if (_schedulePoints[i].HoldedObject == null)
            {
                _schedulePoints[i].HoldedObject = unit;
                break;
            }
        }
    }

    public List<GridUnit> GetAllUnits()
    {
        List<GridUnit> units = new List<GridUnit>();

        foreach (var item in _schedulePoints)
        {
            if(item.HoldedObject != null)
            {
                units.Add(item.HoldedObject);
                item.RemoveUnit();
            }
        }

        return units;
    }
}
