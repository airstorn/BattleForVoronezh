using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace GameStates
{
    public class EnemyTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _nextState;
        [SerializeField] private EnemyRandom _mind;
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;

        private IGameState _state;
        
        private void Start()
        {
            _state = _nextState.GetComponent<IGameState>();
        }

        public void Activate()
        {
            _logic.CameraStatement.ToCamera(_offsetCamera);
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