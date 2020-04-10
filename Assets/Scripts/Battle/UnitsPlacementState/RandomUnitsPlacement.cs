using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomUnitsPlacement : MonoBehaviour, IUnitsPlacer
{
    public void SetPlacableUnits(GridObject grid, GridUnit[] unitsArray)
    {
        for (int i = 0; i < unitsArray.Length; i++)
        {
            //unitsArray[i].Rotate();
            grid.TryPlaceUnit(unitsArray[i]);
        }
    }

    public void Place(GridUnit unit)
    {

    }
}
