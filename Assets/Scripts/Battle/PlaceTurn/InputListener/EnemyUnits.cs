using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnits : MonoBehaviour, IUnitsData
{

    private UnitsData _units;
    private List<GridUnit> _spawnedUnits = new List<GridUnit>();

    public List<GridUnit> GetAllUnits()
    {
        return _spawnedUnits;
    }

    public GridUnit GetUnit(GridUnit choosedUnit)
    {
        throw new System.NotImplementedException();
    }

    public void SetUnit(GridUnit data)
    {
        throw new System.NotImplementedException();
    }

    public void SetUnits(UnitsData data)
    {
        _units = data;

        for (int i = 0; i < _units.Data.Length; i++)
        {
            var unit = Instantiate(_units.Data[i]);
            _spawnedUnits.Add(unit);
        }
    }
}
