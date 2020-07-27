﻿using System;
using System.Collections;
using Battle.Interfaces;
using Cinemachine;
using Core;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameStates
{
    [RequireComponent(typeof(ILevelTarget<GridObject>))]
    public class EnemyTurn : MonoBehaviour, IEnemyState
    {
        [SerializeField] private CinemachineVirtualCamera _offsetCamera;

        private GridElement _selectedElement;

        private GridObject _interactionGrid;
        private IShotable _shot;
        private ILevelTarget<GridObject> _enemyTarget; 
        private IGameState _state;
        
        private void Start()
        {
            _enemyTarget = GetComponent<ILevelTarget<GridObject>>();
            _interactionGrid = LevelData.Instance.PlayerGrid;
            _shot = GetComponent<IShotable>();
            
            _enemyTarget.SetTarget(LevelData.Instance.PlayerGrid);
        }

        public void Activate()
        {
            LevelData.Instance.CameraStatement.ToCamera(_offsetCamera);
            Menu.Instance.SwitchPage<EnemyStatePage>();
            StartCoroutine(Animate());
        }

        private IEnumerator Animate()
        {
            yield return new WaitForSeconds(1);
            
            yield return ShootAtRandomPoint();

            if (_enemyTarget.CheckTarget() == true)
            {
                yield break;
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
                yield return new WaitForSeconds(1);
                shoot = Shoot();
                
                if (_enemyTarget.CheckTarget() == true)
                {
                    yield break;
                }
                
                
            }
            yield return new WaitForSeconds(1);
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
      
            _shot.Release(ref _selectedElement);
            return _selectedElement.HitState == GridSprites.SpriteState.damaged;
        }

        private void EndTurn()
        {
            LevelData.Instance.ChangeState<IPlayerState>();
        }

        public void Deactivate()
        {
            
        }
    }
}