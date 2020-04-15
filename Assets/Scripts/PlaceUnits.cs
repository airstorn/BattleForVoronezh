using System.Collections;
using UnityEngine;

namespace GameStates
{
    public class PlaceUnits : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private GameObject _uiObject;
        [SerializeField] private GameObject _playerTurnState;

        private IInputHandler _inputHandler;

        private void Start()
        {
            _inputHandler = _inputObject.GetComponent<IInputHandler>();
        }
        public void Activate()
        {
            _uiObject.SetActive(true);
            _logic.OnUpdate += StateUpdate;
        }

        private IEnumerator Animate()
        {
            _logic.CameraStatement.ToPlacerCam();
            yield return null;
            //_logic.ChangeState(_logic._state_PlayerTurn);
        }

        public void Deactivate()
        {
            _uiObject.SetActive(false);
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

        private bool ValidatePlacement()
        {

            if (_logic.PlayerGrid.AllUnitsPlaced() == true)
            {
                foreach (var placedElement in _logic.PlayerGrid.Units)
                {
                    if (placedElement.SuitablePlaced == false)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}