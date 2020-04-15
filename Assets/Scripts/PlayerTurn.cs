using UnityEngine;
namespace GameStates
{
    public class PlayerTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private CameraTurns _cameraMovement;
        [SerializeField] private GameObject _inputObject;

        private IInputHandler _inputHandler;


        private void Start()
        {
            _inputHandler = _inputObject.GetComponent<IInputHandler>();
        }
        public void Activate()
        {
            _logic.OnUpdate += StateUpdate;
            _cameraMovement.ToEnemyCam();
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