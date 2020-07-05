using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankAttackPlacer : MonoBehaviour, IUnitsPlacer
{
    [SerializeField] private IUnitsData _storedUnits;
    [SerializeField] private GridObject _interactableGrid;

    private void Start()
    {
        _storedUnits = GetComponent<IUnitsData>();

        _storedUnits.SetUnits(_interactableGrid.UnitsData);

        for (int i = 0; i < _storedUnits.GetAllUnits().Count; i++)
        {
            Place(_storedUnits.GetAllUnits()[i]);
        }

        _interactableGrid.UpdateGridEngagements();
    }

    public void Place(GridUnit unit)
    {
        int step = _interactableGrid.Sheet.GetLength(1) / _storedUnits.GetAllUnits().Count;
        
        for (int y = _interactableGrid.Sheet.GetLength(0) - 1; y > 0; y--)
        {
            for (int i = step / 2; i < _interactableGrid.Sheet.GetLength(1); i += step)
            {

                var posCache = unit.PositionId;
                Vector2Int vacantPos = new Vector2Int(y, i);
                Vector3 pos = _interactableGrid.Sheet[vacantPos.x, vacantPos.y].CellPos;
                unit.PositionId = Vector3Int.RoundToInt(new Vector3(pos.x, 0, pos.z));

                if (_interactableGrid.IsUnitPlacable(unit))
                {
                    _interactableGrid.PlaceUnit(unit, false);
                    break;
                }
                else
                {
                    unit.PositionId = posCache;
                }
            }
        }
    }
}
