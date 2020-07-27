using System;
using System.Linq;
using Abilities.Core;
using Battle.Interfaces;
using Cinemachine;
using Core;
using GameStates;
using Interfaces;
using UnityEngine;
using User;

namespace States.TankAttack
{
    public class PlayerTankTurn : MonoBehaviour, IPlayerState
    {
        [SerializeField] private MultipleTargetsTracker _tracker;
        [SerializeField] private CinemachineVirtualCamera _camera;

        private IInputHandler _inputHandler;
        private IAbilityPresetHandler _abilityHandler;
        private ILevelTarget<GridObject> _target;

        private IInputHandler _defaultHandler;

        private void Start()
        {
            _target = GetComponent<ILevelTarget<GridObject>>().SetTarget(LevelData.Instance.EnemyGrid);
            _abilityHandler = GetComponent<IAbilityPresetHandler>();
            _abilityHandler.Load(UserData.Instance.AbilitiesDirector);
            
            _defaultHandler = _tracker.GetComponent<IInputHandler>();
        }

        public void Activate()
        {
            if (_target.CheckTarget() == true)
            {
                LevelData.Instance.OnPlayerWin?.Invoke();
                return;
            }

            Menu.Instance.SwitchPage<PlayerStatePage>();
            
            LevelData.Instance.OnUpdate += StateUpdate;
            LevelData.Instance.CameraStatement.ToCamera(_camera);

            ResetInput();
            
            if (_inputHandler is MultipleTargetsTracker tracker)
            {
                var count = LevelData.Instance.EnemyGrid.Units.Count(unit => unit.Health.IsDead == false);
                tracker.SetShotsCount(count);
                tracker.OnInputStoppedHandler += TanksTurn;
            }
        }

        private void TanksTurn()
        {
            LevelData.Instance.ChangeState<IEnemyState>();
        }

        private void StateUpdate()
        {
            _inputHandler.TrackInput();
        }

        public void Deactivate()
        {
            LevelData.Instance.OnUpdate -= StateUpdate;
            
            ResetInput();

            _inputHandler.OnInputStoppedHandler -= TanksTurn;
        }

        public void SetInput(IInputHandler handler)
        {
            _inputHandler = handler;
        }

        public void SetInput(IInputHandler handler, bool track)
        {
            throw new NotImplementedException();
        }

        public bool CheckTarget()
        {
            return false;
        }

        public void ResetInput()
        {
            SetInput(_defaultHandler);
        }
    }
}
