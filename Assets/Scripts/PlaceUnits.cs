﻿using System.Collections;
using System.Linq;
using Cinemachine;
using UnityEngine;

namespace GameStates
{
    public class PlaceUnits : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private PageBasement _uiObject;
        [SerializeField] private GameObject _playerTurnState;
        [SerializeField] private CinemachineVirtualCamera _cameraOffset;
        [SerializeField] private UnitsPlacement _playerPlacement;

        private IInputHandler _inputHandler;

        private void Start()
        {
            _inputHandler = _inputObject.GetComponent<IInputHandler>();
        }
        public void Activate()
        {
            _uiObject.Show(this);
            _logic.OnUpdate += StateUpdate;
        }

        private IEnumerator Animate()
        {
            _logic.CameraStatement.ToCamera(_cameraOffset);
            yield return null;
        }

        public void Deactivate()
        {
            _uiObject.Hide();
            
           Destroy(_inputObject);
            
            _logic.OnUpdate -= StateUpdate;
        }

        public void StateUpdate()
        {
            if(Input.GetMouseButtonDown(0))
            {
                _inputHandler.TrackInput();
            }
        }

        public void Confirm()
        {
            if(ValidatePlacement() == true)
            {
                _logic.ChangeState(_playerTurnState.GetComponent<IGameState>());
            }
        }

        public void RotateElement()
        {
            _playerPlacement.RotatePlacebleElement();
        }

        public void PlaceRandomly()
        {
            _playerPlacement.PlaceRandomly();
        }

        private bool ValidatePlacement()
        {
            return _logic.PlayerGrid.AllUnitsPlaced() == true && _logic.PlayerGrid.Units.All(placedElement => placedElement.SuitablePlaced != false);
        }
    }
}