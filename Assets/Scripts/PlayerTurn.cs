using Cinemachine;
using UnityEngine;
namespace GameStates
{
    public class PlayerTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;

        private IInputHandler _inputHandler;


        private void Start()
        {
            _inputHandler = _inputObject.GetComponent<IInputHandler>();
        }
        public void Activate()
        {
            _logic.OnUpdate += StateUpdate;
            _logic.CameraStatement.ToCamera(_offsetCamera);
        }


        public void StateUpdate()
        {
            _inputHandler.TrackInput();
        }

        public void Deactivate()
        {
            _logic.OnUpdate -= StateUpdate;
        }
    }
}