using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameStates
{
    public class EnemyTanksTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _nextState;
        [SerializeField] private GridObject _interactionGrid;
        [SerializeField] private Vector3Int[] _moveDirection;
        [SerializeField] private MultipleTargetsTracker _shots;
        
        private IGameState _state;
        private ILevelTarget _target;

        private void Start()
        {
            _state = _nextState.GetComponent<IGameState>();
            _target = GetComponent<ILevelTarget>();
        }

        public void Activate()
        {
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            
            for (int i = 0; i < _interactionGrid.Units.Count; i++)
            {
                if (_interactionGrid.Units[i].Health.IsDead == false)
                {
                    MoveUnit(_interactionGrid.Units[i]);
                    yield return new WaitForSeconds(1);
                }
            }
            yield return _shots.ShotAnimation();
            
            EndTurn();
        }

        private void MoveUnit(GridUnit unit)
        {
            Vector3Int direction = GetDirection(Random.Range(0, _moveDirection.Length));
            var fromPos =  unit.PositionId;
            bool moved = false;

            for (int i = 0; i < _moveDirection.Length; i++)
            {
                var element = _interactionGrid.GetVacantElement(fromPos + direction);

                if (element != null)
                {
                    var pos = element.CellPos;

                    unit.PositionId = Vector3Int.RoundToInt(new Vector3(pos.x, 0, pos.z));

                    if (_interactionGrid.IsUnitPlacable(unit))
                    {
                        _interactionGrid.PlaceUnit(unit, true);
                        moved = true;
                        break;
                    }
                }
                direction = GetDirection(i);
                Debug.Log(i);
            }

            if (moved == false)
            {
                unit.PositionId = fromPos;
                _interactionGrid.PlaceUnit(unit, false);
            }
        }

        private Vector3Int GetDirection(int id)
        {
            return _moveDirection[Mathf.Clamp(id, 0, _moveDirection.Length)];
        }

        private void EndTurn()
        {
            if (_target.CheckTarget() == true)
            {
                _logic.OnPlayerLoose?.Invoke();
            }
            _logic.ChangeState(_state);
        }

        public void Deactivate()
        {
        }
    }
}
