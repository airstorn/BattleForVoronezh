using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.Interfaces;
using UnityEngine;

namespace levelTarget
{
    public class TargetClearField : MonoBehaviour, ILevelTarget<GridObject>
    {
        private GridObject _targetGrid;
    
        public bool CheckTarget()
        {
            return _targetGrid.Units.All(killedUnit => killedUnit.Health.IsDead);
        }

        public ILevelTarget<GridObject> SetTarget(GridObject target)
        {
            _targetGrid = target;
            return this;
        }

        public GridObject GetTarget()
        {
            return _targetGrid;
        }
    }
}