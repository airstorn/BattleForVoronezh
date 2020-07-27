using System;
using System.Linq;
using Battle.Interfaces;
using Core;
using InputHandlers;
using Interfaces;
using UnityEngine;

namespace LevelTargets
{
    public class ArtPreparationTarget : MonoBehaviour, ILevelTarget<GridObject>
    {
        private GridObject _targetGrid;
        private LimitedShotsHandler _handler;
        
        public struct ArtPreparationTargetData
        {
            public GridObject Target;
        }

        private void Start()
        {
            _handler = GetComponentInChildren<LimitedShotsHandler>();
        }

        public bool CheckTarget()
        {
            var kills = _targetGrid.Units.All(killedUnit => killedUnit.Health.IsDead);
            var shots = _handler.ShotsCount == 0;
            
            if (kills == true)
            {
                LevelData.Instance.OnPlayerWin?.Invoke();
            }
            else if (shots == true)
            {
                LevelData.Instance.OnPlayerLoose?.Invoke();
            }

            return kills || shots;
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