using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class ScheduleObject
{
    public Vector3 Pos;
    [SerializeField] private GridUnit _holdedObject;
    public GridUnit HoldedObject
    {
        get
        {
            return _holdedObject;
        }
        set
        {
            _holdedObject = value;
            if(value != null)
                _holdedObject.transform.DOMove(Pos, 1);
        }
    }

    public void RemoveUnit()
    {
        _holdedObject.OnDrag -= RemoveUnit;
        _holdedObject = null;
    }
}

public class UnitsManager : MonoBehaviour
{
    [SerializeField] private UnitsData _units;
    [SerializeField] private Transform _selectPoint;
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactableGrid;

    public ScheduleObject[] _schedulePoints;
    private GridUnit _currentUnit;
    private Tween _moveToPointTween;

    public void InitUnits()
    {
        PoolSchedulePoints();

        foreach (var unit in _units.Data)
        {
            var createdObject = Instantiate(unit.gameObject);
            AddUnitToSchedule(createdObject.GetComponent<GridUnit>());
        }
    }

    public void CatchUnit()
    {
        RaycastHit hit;
        if (Physics.Raycast(_raycastCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.CompareTag("Unit"))
            {
                _currentUnit = hit.collider.GetComponent<GridUnit>();
                _currentUnit.OnDrag?.Invoke();

                _interactableGrid.RemoveUnit(_currentUnit);

                StopCoroutine(DragUnit());
                StartCoroutine(DragUnit());
            }
        }
    }

    private IEnumerator DragUnit()
    {
        bool drag = true;

        while (drag == true)
        {
            _interactableGrid.PredictPlace(_currentUnit, GridObject.ElementState.vacant);

            Vector3 pos = new Vector3(
                   _raycastCamera.ScreenToWorldPoint(Input.mousePosition).x,
                   0.5f,
                   _raycastCamera.ScreenToWorldPoint(Input.mousePosition).z
                   );

            if (_currentUnit != null)
                _currentUnit.transform.position = pos;

            if(Input.GetMouseButtonUp(0))
            {
                drag = false;

                if(_interactableGrid.TryPlaceUnit(_currentUnit) == true)
                {
                    _interactableGrid.PlaceUnit(_currentUnit);
                }
                else
                {
                    AddUnitToSchedule(_currentUnit);
                }
            }
            yield return null;
        }
    }

    private void AddUnitToSchedule(GridUnit unit)
    {
        for (int i = 0; i < _schedulePoints.Length; i++)
        {
            if(_schedulePoints[i].HoldedObject == null)
            {
                _schedulePoints[i].HoldedObject = unit;
                unit.OnDrag += _schedulePoints[i].RemoveUnit;
                unit.OnDrag += UpdateSchedule;
                break;
            }
        }
    }

    private void UpdateSchedule()
    {
        for (int i = 0; i < _schedulePoints.Length - 1; i++)
        {
            if(_schedulePoints[i].HoldedObject == null && _schedulePoints[i + 1].HoldedObject != null)
            {
                AddUnitToSchedule(_schedulePoints[i + 1].HoldedObject);
                _schedulePoints[i + 1].RemoveUnit();
            }
        }
    }

    private void PoolSchedulePoints()
    {
        _schedulePoints = new ScheduleObject[_units.Data.Length];
        for (int i = 0; i < _schedulePoints.Length; i++)
        {
            _schedulePoints[i] = new ScheduleObject();
            _schedulePoints[i].Pos = new Vector3(_selectPoint.position.x + i * 5, _selectPoint.position.y, _selectPoint.position.z );
        }
    }

    public void RotatePlacebleElement()
    {
        if (_currentUnit != null)
        {
            _currentUnit.Rotate();

            if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
            {
                _interactableGrid.PlaceUnit(_currentUnit);
            }
            else
            {

            }
        }
    }
}
