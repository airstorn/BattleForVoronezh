using System;
using System.Collections;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameStates
{
    [RequireComponent(typeof(ILevelTarget))]
    public class EnemyTurn : MonoBehaviour, IGameState
    {
        [SerializeField] private GameLogic _logic;
        [SerializeField] private GameObject _nextState;
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;
        [SerializeField] private GridObject _interactionGrid;

        private GridElement _selectedElement;

        private IShotable _shot;
        private ILevelTarget _enemyTarget; 
        private IGameState _state;
        
        private void Start()
        {
            _state = _nextState.GetComponent<IGameState>();
            _enemyTarget = GetComponent<ILevelTarget>();
            _shot = GetComponent<IShotable>();
        }

        public void Activate()
        {
            _logic.CameraStatement.ToCamera(_offsetCamera);
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            yield return new WaitForSeconds(1);
            
            yield return ShootAtRandomPoint();

            if (_enemyTarget.CheckTarget() == true)
            {
                yield return new WaitForSeconds(1);
                _logic.OnPlayerLoose?.Invoke();
            }
            else
            {
                yield return new WaitForSeconds(1);
                EndTurn();
            }
        }
        
        public IEnumerator ShootAtRandomPoint()
        {
            bool shoot = true;

            while (shoot == true)
            {
                if (_enemyTarget.CheckTarget() == true)
                {
                    yield break;
                }
                
                yield return new WaitForSeconds(1);
                shoot = Shoot();
            }
            yield return new WaitForSeconds(2);
        }
        
        private Vector2Int RandomizedPoint()
        {
            return new Vector2Int(Random.Range(0, _interactionGrid.Sheet.GetLength(0)),Random.Range(0, _interactionGrid.Sheet.GetLength(1) ));
        }
        
        private bool Shoot()
        {
            Vector2Int randomPointId = RandomizedPoint();
            _selectedElement = _interactionGrid.Sheet[randomPointId.x, randomPointId.y];
      
            while (_selectedElement.HitState != GridSprites.SpriteState.normal)
            {
                randomPointId = RandomizedPoint();
                _selectedElement = _interactionGrid.Sheet[randomPointId.x, randomPointId.y];
            }
      
            _shot.Release(_interactionGrid.Sheet[randomPointId.x, randomPointId.y].CellPos, ref _selectedElement);
            return _selectedElement.HitState == GridSprites.SpriteState.damaged;
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