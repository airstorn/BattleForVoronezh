using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class UnitsPlacement : MonoBehaviour, IInputHandler
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactableGrid;
    [SerializeField] private LayerMask _ignoreMask;
    [SerializeField] private IUnitsData _unitsHolder;
    
    private GridUnit _currentUnit;

    private void Start()
    {
        _unitsHolder = GetComponent<IUnitsData>();
    }

    public void CatchUnit()
    {
        RaycastHit hit;
        if (Physics.Raycast(_raycastCamera.ScreenPointToRay(Input.mousePosition), out hit, _ignoreMask))
        {
            if (hit.collider.GetComponent<GridUnit>() != null)
            {
                _currentUnit = TryGetUnitFromHolder(hit.collider.GetComponent<GridUnit>());

                _interactableGrid.RemoveUnit(_currentUnit);

                StopCoroutine(DragUnit());
                StartCoroutine(DragUnit());
            }
        }
    }

    private GridUnit TryGetUnitFromHolder(GridUnit vacantUnit)
    {
        var unit = _unitsHolder.GetUnit(vacantUnit);

        return unit != null ? unit : vacantUnit;
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

            if (_currentUnit)
                _currentUnit.transform.position = pos;

            if(Input.GetMouseButtonUp(0))
            {
                drag = false;

                ConvertUnitPositionInId();

                if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
                {
                    _interactableGrid.PlaceUnit(_currentUnit);
                }
                else
                {
                    _unitsHolder.SetUnit(_currentUnit);
                }
            }
            yield return null;
        }
    }

    private void ConvertUnitPositionInId()
    {
        if (!_currentUnit)
            return;

        _currentUnit.PositionId = Vector3Int.RoundToInt(_currentUnit.transform.position);
    }

    public void RotatePlacebleElement()
    {
        if (_currentUnit == null) return;
        _currentUnit.Rotate(false);

        if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
        {
            _interactableGrid.PlaceUnit(_currentUnit);
        }
        else
        {
            _interactableGrid.PlaceUnit(_currentUnit);
            var elements = _interactableGrid.GetVacantElements(_currentUnit.PositionId, _currentUnit.Size, _currentUnit.GetDirection(), 0);
            _interactableGrid.SetElementsState(elements, GridObject.ElementState.locked);
        }
    }

    public void PredictPlace(GridUnit unit)
    {
        ConvertUnitPositionInId();

        var vacantElements = _interactableGrid.GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 0);
        var elementOutline = _interactableGrid.GetVacantElements(unit.PositionId, unit.Size, unit.GetDirection(), 1);

        _interactableGrid.UpdateGridEngagements();

        var locked = elementOutline.Any(element => element.HoldedUnit != unit && element.HoldedUnit || vacantElements.Count != unit.Size.x * unit.Size.y);

        _interactableGrid.SetElementsState(vacantElements, locked == true ? GridObject.ElementState.locked : GridObject.ElementState.vacant);
    }

    public void PlaceRandomly()
    {
        foreach (var unitOnGrid in _interactableGrid.Units)
        {
            _unitsHolder.SetUnit(unitOnGrid);
        }

        _interactableGrid.Clear();

        RandomUnitsPlacement randomUnitsPlacement = new RandomUnitsPlacement();

        randomUnitsPlacement.ExecuteUnitsForPlacement(_unitsHolder.GetAllUnits(), _interactableGrid);
    }

    public void TrackInput()
    {
        CatchUnit();
    }
}
