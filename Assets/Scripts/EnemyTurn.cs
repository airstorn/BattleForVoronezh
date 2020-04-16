using System;
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

        private IGameState _state;
        
        private void Start()
        {
            _state = _nextState.GetComponent<IGameState>();
        }

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
            _logic.ChangeState(_state);
        }

        public void Deactivate()
        {
            
        }
    }
}