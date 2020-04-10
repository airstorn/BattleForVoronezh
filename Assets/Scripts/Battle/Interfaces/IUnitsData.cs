using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnitsData
{
    GridUnit GetUnit(GridUnit choosedUnit);
    GridUnit GetUnit(int id);
    void SetUnit(GridUnit data);
    void SetUnits(UnitsData data);
}
