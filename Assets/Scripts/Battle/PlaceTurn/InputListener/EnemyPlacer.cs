using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour, IUnitsPlacer
{
    [SerializeField] private IUnitsData _storedUnits;
    [SerializeField] private UnitsData _defaultUnits;
    [SerializeField] private GridObject _enemyGrid;

    private void Start()
    {
        _storedUnits = GetComponent<IUnitsData>();

        if(_defaultUnits != null)
        {
            _storedUnits.SetUnits(_defaultUnits);
            Place(null);
        }
    }

    public void Place(GridUnit unit)
    {
        RandomUnitsPlacement randomUnitsPlacement = new RandomUnitsPlacement();
        randomUnitsPlacement.ExecuteUnitsForPlacement(_storedUnits.GetAllUnits(), _enemyGrid);
    }
}
