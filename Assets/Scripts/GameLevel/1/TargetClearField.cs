using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetClearField : MonoBehaviour, ILevelTarget
{
    [SerializeField] private GridObject _enemyGrid;
    
    public bool CheckTarget()
    {
        return _enemyGrid.Units.All(killedUnit => killedUnit.Health.IsDead);
    }
}
