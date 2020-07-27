using System;
using Abilities.Core;
using Abilities.Presets;
using Battle.Interfaces;
using Cinemachine;
using Core;
using GameStates;
using InputHandlers;
using Interfaces;
using levelTarget;
using LevelTargets;
using UnityEngine;
using User;

namespace States.Artillery
{
    public class PlayerTurn : MonoBehaviour,  IPlayerState
    {
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;

        public IInputHandler InputHandler => _inputHandler;
        private ILevelTarget<GridObject> _target;
        
        private IInputHandler _inputHandler;
        private IAbilityPresetHandler _abilitiesHandler;


        private void Start()
        {
            
            ResetInput();

            if (_inputHandler is SimpleInputShooting)
            {
                if(GetComponent<Abilities.Presets.Artillery>() == null)
                    gameObject.AddComponent<Abilities.Presets.Artillery>();
                _target = gameObject.AddComponent<TargetClearField>().SetTarget(LevelData.Instance.EnemyGrid);
            }

            if (_inputHandler is LimitedShotsHandler)
            {
                if(GetComponent<ArtilleryPreparation>() == null)
                    gameObject.AddComponent<ArtilleryPreparation>();
                _target = gameObject.AddComponent<ArtPreparationTarget>().SetTarget(LevelData.Instance.EnemyGrid);
            }

           
            _abilitiesHandler = GetComponent<IAbilityPresetHandler>();

            _abilitiesHandler.Load(UserData.Instance.AbilitiesDirector);
        }


        public void Activate()
        {
            LevelData.Instance.OnUpdate += StateUpdate;
            LevelData.Instance.CameraStatement.ToCamera(_offsetCamera);

            Menu.Instance.SwitchPage<PlayerStatePage>();

            SetInput(_inputObject.GetComponent<IInputHandler>(), true);
        }

        public void SetInput(IInputHandler handler, bool trackInput)
        {
            if(_inputHandler != null)
                _inputHandler.OnInputStoppedHandler -= InputCancel;
            _inputHandler = handler;
            
            if(trackInput)
                _inputHandler.OnInputStoppedHandler += InputCancel;
        }

        private void InputCancel()
        {
            LevelData.Instance.ChangeState<IEnemyState>();
        }

        public bool CheckTarget()
        {
            return _target.CheckTarget();
        }

       
        public void ResetInput()
        {
            SetInput(_inputObject.GetComponent<IInputHandler>(), true);
        }

        private void StateUpdate()
        {
            _inputHandler.TrackInput();
        }

        public void Deactivate()
        {
            Debug.Log("deactivate");
            LevelData.Instance.OnUpdate -= StateUpdate;
        }
    }
}