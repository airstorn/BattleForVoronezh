using System.Linq;
using Battle.Interfaces;
using Core;
using Interfaces;
using UnityEngine;

namespace LevelTargets
{
    public class TargetClearField : MonoBehaviour, ILevelTarget<GridObject>
    {
        private GridObject _targetGrid;
    
        public bool CheckTarget()
        {
            var state = _targetGrid.Units.All(killedUnit => killedUnit.Health.IsDead);

            if (state == true)
            {
                LevelData.Instance.OnPlayerWin?.Invoke();
            }

            return state;
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