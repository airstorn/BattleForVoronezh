using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;

public struct PlaceComparerJob
{ 
    public GridUnit Unit;
    public GridObject SelectedGrid;

    public void Execute()
    {
        if (Unit == null)
            return;

        for (int x = 0; x < SelectedGrid.Sheet.GetLength(0); x++)
        {
            for (int y = 0; y < SelectedGrid.Sheet.GetLength(1); y++)
            {
                if(SelectedGrid.Sheet[x,y].HoldedUnit == null)
                {
                    Unit.PositionId = new Vector3Int(x, 0, y);

                    if (SelectedGrid.TryPlaceUnit(Unit))
                    {
                        Unit.OnDrag?.Invoke();
                        SelectedGrid.PlaceUnit(Unit);
                        Debug.Log("Placed");
                        return;
                    }
                    else
                    {
                        continue;
                    }
                }
            }
        }
    }
}

public class RandomUnitsPlacement : IUnitsPlacer
{
    public void ExecuteUnitsForPlacement(GridUnit units, GridObject grid)
    {
            var job = new PlaceComparerJob()
            {
                Unit = units,
                SelectedGrid = grid
            };
        job.Execute();
    }

    public void Place(GridUnit unit)
    {
    }
}
