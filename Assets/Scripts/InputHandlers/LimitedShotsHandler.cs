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

        [SerializeField] private GameObject _template;
        [SerializeField] private Transform _parent;
        [SerializeField] private int _additionalShots = 0;
        [SerializeField] private bool _useFixedShots = false;
        [SerializeField] private int _fixedShots;

        public int ShotsCount => _shotsCount;
        
        private Text _shotsText;
        private int _shotsCount;
        private GridObject _interactionTarget;
        public event OnInputStopped OnInputStoppedHandler;

        private Camera _raycastCamera;
        private EventSystem _eventSystem;

        private IShotable _shotBehaviour;
        private bool animate = false;
        
        private IEnumerator Start()
        {
            _shotBehaviour = GetComponent<IShotable>();
            _raycastCamera = Camera.main;
            _interactionTarget = LevelData.Instance.EnemyGrid;
        
            _eventSystem = EventSystem.current;
            
            _shotsText = Instantiate(_template, _parent).GetComponent<Text>();

            yield return new WaitForEndOfFrame();

            var target = LevelData.Instance.EnemyGrid;

            if (_useFixedShots == false)
            {
                _shotsCount = target.Units.Sum(unit => unit.Health.Total) * (target.Sheet.GetLength(0) + target.Sheet.GetLength(1)) / 2;
                _shotsCount += _additionalShots;
            }
            else
            {
                _shotsCount = _fixedShots;
            }
          
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
                        var selectedElement = HighlightPoint(roundedPos);
                    
                        if(selectedElement.HitState == GridSprites.SpriteState.normal)
                            Shoot(selectedElement);
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
            _shotsText.text = Lean.Localization.LeanLocalization.GetTranslationText("Ammo") + " " + _shotsCount;
        }

        private GridElement HighlightPoint(Vector3Int pos)
        {
            return _interactionTarget.GetVacantElement(pos);
        }

        public void Shoot(GridElement selectedElement)
        {
            StartCoroutine(ShotAnimation(selectedElement));
        }

        private IEnumerator ShotAnimation(GridElement selectedElement)
        {
            animate = true;
        
            _shotBehaviour.Release( ref selectedElement);
            RemoveShot();
            yield return new WaitForSeconds(0.8f);

            if (LevelData.Instance.PlayerState.CheckTarget() == true)
                yield break;
                
            if(selectedElement.HitState == GridSprites.SpriteState.missed)
                OnInputStoppedHandler?.Invoke();
            
            animate = false;
        }
    }
}
