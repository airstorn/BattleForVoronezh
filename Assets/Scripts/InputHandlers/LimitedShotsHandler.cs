using System.Collections;
using System.Linq;
using Battle.Interfaces;
using Core;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InputHandlers
{
    public class LimitedShotsHandler : MonoBehaviour, IInputHandler
    {
        [SerializeField] private LayerMask _raycastIgnore;
        [SerializeField] private GameObject _enemyTurn;

        [SerializeField] private GameObject _template;
        [SerializeField] private Transform _parent;
        [SerializeField] private int _additionalShots = 0;

        private Text _shotsText;
        private int _shotsCount;
        private GridElement _selectedElement;
        public event OnInputStopped OnInputStoppedHandler;

        private Camera _raycastCamera;
        private EventSystem _eventSystem;

        private ILevelTarget<GridObject> _target;
        private IShotable _shotBehaviour;
        private IGameState _nextState;
        private bool animate = false;
        
        private IEnumerator Start()
        {
            _shotBehaviour = GetComponent<IShotable>();
            _nextState = _enemyTurn.GetComponent<IGameState>();
            _target = GetComponent<ILevelTarget<GridObject>>();
            _raycastCamera = Camera.main;
        
            _eventSystem = EventSystem.current;
            
            _target.SetTarget(LevelData.Instance.EnemyGrid);

            _shotsText = Instantiate(_template, _parent).GetComponent<Text>();

            yield return new WaitForEndOfFrame();

            var target = _target.GetTarget();
        
            _shotsCount = target.Units.Sum(unit => unit.Health.Total) * (target.Sheet.GetLength(0) + target.Sheet.GetLength(1)) / 2;
            _shotsCount += _additionalShots;
            UpdateShotsCount();
        }
    
        public void TrackInput()
        {
            var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0) && animate == false && _eventSystem.IsPointerOverGameObject() == false)
            {
                if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
                {
                    if (hit.collider)
                    {
                        Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                        _selectedElement = HighlightPoint(roundedPos);
                    
                        if(_selectedElement.HitState == GridSprites.SpriteState.normal)
                            Shoot();
                    }
                }
            }
        }

        public void AddShots(int count)
        {
            _shotsCount += count;
            UpdateShotsCount();
        }

        private void RemoveShot()
        {
            _shotsCount--;
            UpdateShotsCount();
        }

        private void UpdateShotsCount()
        {
            _shotsText.text = "Боеприпасы: " + _shotsCount;
        }

        private GridElement HighlightPoint(Vector3Int pos)
        {
            return _target.GetTarget().GetVacantElement(pos);
        }

        public void Shoot()
        {
            StartCoroutine(ShotAnimation());
        }

        private IEnumerator ShotAnimation()
        {
            animate = true;
        
            _shotBehaviour.Release( ref _selectedElement);
            RemoveShot();
            yield return new WaitForSeconds(0.8f);

            if (_target.CheckTarget() == true)
            {
                LevelData.Instance.OnPlayerWin?.Invoke();
                yield break;
            }
        
            if (_shotsCount <= 0)
            {
                LevelData.Instance.OnPlayerLoose?.Invoke();
                yield break;
            }
        
            if(_selectedElement.HitState == GridSprites.SpriteState.missed)
                LevelData.Instance.ChangeState(_nextState);

            animate = false;
        }
    }
}
