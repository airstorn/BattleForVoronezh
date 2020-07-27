using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class EnemyPlacer : MonoBehaviour, IUnitsPlacer
{
    [SerializeField] private IUnitsData _storedUnits;

    private void Start()
    {
        _storedUnits = GetComponent<IUnitsData>();

        _storedUnits.SetUnits(LevelData.Instance.EnemyGrid.UnitsData);
        Place(null);
        LevelData.Instance.EnemyGrid.UpdateGridEngagements();
    }

    public void Place(GridUnit unit)
    {
        RandomUnitsPlacement randomUnitsPlacement = new RandomUnitsPlacement();
        randomUnitsPlacement.ExecuteUnitsForPlacement(_storedUnits.GetAllUnits(), LevelData.Instance.EnemyGrid);
    }
}
