using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;
using DG.Tweening;
using System;

public struct PlaceComparerJob
{ 
    public GridUnit Unit;
    public GridObject SelectedGrid;

    public void Execute()
    {
        for (int i = 0; i < UnityEngine.Random.Range(0, (int)4); i++)
            Unit.Rotate(true);
        do
        {
            Vector2Int randomPos = new Vector2Int(UnityEngine.Random.Range(0, (int)SelectedGrid.Sheet.GetLength(0)), UnityEngine.Random.Range(0, (int)SelectedGrid.Sheet.GetLength(1)));

            Unit.PositionId = new Vector3Int(randomPos.x, 0, randomPos.y);
            if (SelectedGrid.TryPlaceUnit(Unit) == true)
            {
                SelectedGrid.PlaceUnit(Unit);
            }
        }
        while (Unit.SuitablePlaced == false);

    }
}

public class RandomUnitsPlacement : IUnitsPlacer
{
    public void ExecuteUnitsForPlacement(List<GridUnit> units, GridObject grid)
    {
        units = units.OrderByDescending(x => x.Size.x * x.Size.y).ToList();

        for (int i = 0; i < units.Count; i++)
        {
            Debug.Log(units[i]);
            var PlaceJob = new PlaceComparerJob()
            {
                SelectedGrid = grid,
                Unit = units[i]
            };

            PlaceJob.Execute();
        }
    }

    public void Place(GridUnit unit)
    {
    }
}
