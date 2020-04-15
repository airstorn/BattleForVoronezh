using System.Collections;
using UnityEngine;

namespace GameStates
{
    public class EnemyTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private CameraTurns _cameraStatement;
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _nextState;
        [SerializeField] private EnemyRandom _mind;
        
        public void Activate()
        {
            _cameraStatement.ToPlayerCam();
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            yield return new WaitForSeconds(1);
            yield return _mind.ShootAtRandomPoint();
            yield return new WaitForSeconds(1);
            EndTurn();
        }

        private void EndTurn()
        {
            _logic.ChangeState(_nextState.GetComponent<IGameState>());
        }

        public void Deactivate()
        {
            
        }
    }
}