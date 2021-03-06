﻿using System.Collections;
using Battle.Interfaces;
using Core;
using GameStates;
using GUI.Core;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace States.TankAttack
{
    public class EnemyTanksTurn : MonoBehaviour, IEnemyState
    {
        [SerializeField] private MultipleTargetsTracker _shots;

        [SerializeField] private Vector3Int[] _moveDirection = new []{new Vector3Int(1,0, -1), new Vector3Int(1,0,0), new Vector3Int(1,0, 1), };
        
        private ILevelTarget<GridObject> _target;

        private void Start()
        {
            _target = GetComponent<ILevelTarget<GridObject>>().SetTarget(LevelData.Instance.EnemyGrid);
        }

        public void Activate()
        {
            Menu.Instance.SwitchPage<EnemyStatePage>();
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            var target = _target.GetTarget();
            
            for (int i = target.Units.Count - 1; i >= 0; i--)
            {
                if (target.Units[i].Health.IsDead == false)
                {
                    MoveUnit(target.Units[i]);
                    yield return new WaitForSeconds(0.75f);
                }
            }
            yield return _shots.ShotAnimation(EndTurn);
            
        
        }

        private void MoveUnit(GridUnit unit)
        {
            Vector3Int direction = GetDirection(Random.Range(0, _moveDirection.Length));
            var fromPos =  unit.PositionId;
            var target = _target.GetTarget();
            bool moved = false;

            for (int i = 0; i < _moveDirection.Length; i++)
            {
                var element = target.GetVacantElement(fromPos + direction);

                if (element != null)
                {
                    var pos = element.CellPos;

                    unit.PositionId = Vector3Int.RoundToInt(new Vector3(pos.x, 0, pos.z));

                    if (target.IsUnitPlacable(unit))
                    {
                        target.PlaceUnit(unit, true);
                        moved = true;
                        break;
                    }
                }
                direction = GetDirection(i);
            }

            if (moved == false)
            {
                unit.PositionId = fromPos;
                target.PlaceUnit(unit, false);
            }
            
            SoundsPlayer.Instance.PlaySound(SoundType.Tank);
        }

        private Vector3Int GetDirection(int id)
        {
            return _moveDirection[Mathf.Clamp(id, 0, _moveDirection.Length)];
        }

        private void EndTurn()
        {
            if (_target.CheckTarget() == true)
            {
                LevelData.Instance.OnPlayerLoose?.Invoke();
            }
            else
            {
                LevelData.Instance.ChangeState<IPlayerState>();
            }
        }

        public void Deactivate()
        {
        }
    }
}
