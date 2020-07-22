using Abilities.Core;
using Cinemachine;
using UnityEngine;
namespace GameStates
{
    public class PlayerTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameObject _inputObject;
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;

        private IInputHandler _inputHandler;
        private IAbilityPresetHandler _abilitiesHandler;


        private void Start()
        {
            _inputHandler = _inputObject.GetComponent<IInputHandler>();
            _abilitiesHandler = GetComponent<IAbilityPresetHandler>();

            _abilitiesHandler.Load(UserData.Instance.AbilitiesDirector);
        }
        public void Activate()
        {
            LevelData.Instance.OnUpdate += StateUpdate;
            LevelData.Instance.CameraStatement.ToCamera(_offsetCamera);
        }


        public void StateUpdate()
        {
            _inputHandler.TrackInput();
        }

        public void Deactivate()
        {
            LevelData.Instance.OnUpdate -= StateUpdate;
        }
    }
}