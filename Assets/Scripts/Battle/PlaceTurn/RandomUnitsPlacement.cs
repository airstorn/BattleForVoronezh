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

    public void Execute(int rotationIndex)
    {
        if (Unit.Rotation != rotationIndex * 90)
        {
            for (int i = 0; i < rotationIndex; i++)
            {
                Unit.Rotate(true);
            }
        }
       

        int _attempts = 500;
        int _currentAttempts = 0;
        do
        {
            Vector2Int randomPos = new Vector2Int(UnityEngine.Random.Range(0, (int)SelectedGrid.Sheet.GetLength(0)), UnityEngine.Random.Range(0, (int)SelectedGrid.Sheet.GetLength(1)));
            Vector3 pos = SelectedGrid.Sheet[randomPos.x, randomPos.y].CellPos;

            Unit.PositionId = Vector3Int.RoundToInt(new Vector3(pos.x, 0, pos.z));
            if (SelectedGrid.IsUnitPlacable(Unit) == true)
            {
                SelectedGrid.PlaceUnit(Unit, false);
                return;
            }

            _currentAttempts++;
        }
        while (Unit.SuitablePlaced == false && _currentAttempts < _attempts);
        

        for (int x = 0; x < SelectedGrid.Sheet.GetLength(0); x++)
        {
            for (int y = 0; y < SelectedGrid.Sheet.GetLength(1); y++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Unit.Rotate(true);
                    
                    Vector2Int vacantPos = new Vector2Int(x,y);
                    Vector3 pos = SelectedGrid.Sheet[vacantPos.x, vacantPos.y].CellPos;
                    Unit.PositionId = Vector3Int.RoundToInt(new Vector3(pos.x, 0, pos.z));
                    
                    if (SelectedGrid.IsUnitPlacable(Unit) == true)
                    {
                        SelectedGrid.PlaceUnit(Unit, false);
                        return;
                    }
                }
            }
        } 
        
        throw new NotImplementedException("Cant place unit on grid" + SelectedGrid.name);
    }
}

public class RandomUnitsPlacement 
{
    public void ExecuteUnitsForPlacement(List<GridUnit> units, GridObject grid, int rotationIndex)
    {
        units = units.OrderByDescending(x => x.Size.x * x.Size.y).ToList();

        for (int i = 0; i < units.Count; i++)
        {
            var PlaceJob = new PlaceComparerJob()
            {
                SelectedGrid = grid,
                Unit = units[i]
            };

            PlaceJob.Execute(rotationIndex);
        }
    }
}
