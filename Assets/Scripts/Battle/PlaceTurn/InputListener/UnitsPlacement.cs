using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitsPlacement : MonoBehaviour, IInputHandler
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactableGrid;
    [SerializeField] private IUnitsData _unitsHolder;
    
    private GridUnit _currentUnit;

    private void Start()
    {
        _unitsHolder = GetComponent<IUnitsData>();
    }

    public void CatchUnit()
    {
        RaycastHit hit;
        if (Physics.Raycast(_raycastCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.GetComponent<GridUnit>() != null)
            {
                _currentUnit = TryGetUnitFromHolder(hit.collider.GetComponent<GridUnit>());

                _currentUnit.OnDrag?.Invoke();

                _interactableGrid.RemoveUnit(_currentUnit);

                StopCoroutine(DragUnit());
                StartCoroutine(DragUnit());
            }
        }
    }

    private GridUnit TryGetUnitFromHolder(GridUnit vacantUnit)
    {
        var unit = _unitsHolder.GetUnit(vacantUnit);

        if (unit != null)
            return unit;
        else
            return vacantUnit;
    }

    private IEnumerator DragUnit()
    {
        bool drag = true;

        while (drag == true)
        {
            PredictPlace(_currentUnit);

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

                ConvertUnitPositionInId();

                if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
                {
                    _currentUnit.SuitablePlaced = true;
                    _interactableGrid.PlaceUnit(_currentUnit);
                }
                else
                {
                    _currentUnit.SuitablePlaced = false;
                    _unitsHolder.SetUnit(_currentUnit);
                }
            }
            yield return null;
        }
    }

    private void ConvertUnitPositionInId()
    {
        if (_currentUnit == null)
            return;

        _currentUnit.PositionId = Vector3Int.RoundToInt(_currentUnit.transform.position);
    }

    public void RotatePlacebleElement()
    {
        if (_currentUnit != null)
        {
            _currentUnit.Rotate();

            if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
            {
                _currentUnit.SuitablePlaced = true;
                _interactableGrid.PlaceUnit(_currentUnit);
            }
            else
            {
                _currentUnit.SuitablePlaced = false;
                _interactableGrid.PlaceUnit(_currentUnit);
            }
        }
    }

    public void PredictPlace(GridUnit unit)
    {
        ConvertUnitPositionInId();

        List<GridElement> vacantElements = _interactableGrid.GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 0);

        _interactableGrid.UpdateGridEngagements();

        bool locked = false;

        foreach (var element in vacantElements)
        {
            if (element.HoldedUnit != unit && element.HoldedUnit != null || vacantElements.Count != unit.Size.x * unit.Size.y)
            {
                locked = true;
                break;
            }
        }
        _interactableGrid.SetElementsState(vacantElements, locked == true ? GridObject.ElementState.locked : GridObject.ElementState.vacant);
    }

    public void PlaceRandomly()
    {
        RandomUnitsPlacement randomUnitsPlacement = new RandomUnitsPlacement();

        randomUnitsPlacement.ExecuteUnitsForPlacement(_unitsHolder.GetUnit(0), _interactableGrid);
    }

    public void TrackInput()
    {
        CatchUnit();
    }
}
