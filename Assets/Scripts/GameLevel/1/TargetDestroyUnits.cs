using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TargetDestroyUnits : MonoBehaviour, ILevelTarget
{
    [SerializeField] private List<GridUnit> _targetUnits = new List<GridUnit>();
    [SerializeField] private GridObject _grid;
    [SerializeField] private List<GridUnit> _units = new List<GridUnit>();

    public bool CheckTarget()
    {
        return _units.Any(unit => unit.Health.IsDead);
    }
}
