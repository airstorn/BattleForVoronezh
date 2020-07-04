using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour, IUnitsPlacer
{
    [SerializeField] private IUnitsData _storedUnits;
    [SerializeField] private GridObject _enemyGrid;

    private void Start()
    {
        _storedUnits = GetComponent<IUnitsData>();

        _storedUnits.SetUnits(_enemyGrid.UnitsData);
        Place(null);
        _enemyGrid.UpdateGridEngagements();
    }

    public void Place(GridUnit unit)
    {
        RandomUnitsPlacement randomUnitsPlacement = new RandomUnitsPlacement();
        randomUnitsPlacement.ExecuteUnitsForPlacement(_storedUnits.GetAllUnits(), _enemyGrid);
    }
}
