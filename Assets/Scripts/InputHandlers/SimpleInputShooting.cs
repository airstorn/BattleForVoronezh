using System.Collections;
using Core;
using Interfaces;
using UnityEngine;

namespace InputHandlers
{
    public class SimpleInputShooting : MonoBehaviour, IInputHandler
    {
        public event OnInputStopped OnInputStoppedHandler;
    
        [SerializeField] private LayerMask _raycastIgnore;
    
        private Camera _raycastCamera;
        private GridObject _interactionGrid;

        private IShotable _shotBehaviour;
        private bool animate = false;
    
        private void Start()
        {
            _shotBehaviour = GetComponent<IShotable>();
            _interactionGrid = LevelData.Instance.EnemyGrid;
            _raycastCamera = Camera.main;
        }
    
        public void TrackInput()
        {
            var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Input.GetMouseButtonDown(0) && animate == false)
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


        private GridElement HighlightPoint(Vector3Int pos)
        {
            return _interactionGrid.GetVacantElement(pos);
        }

        public void Shoot(GridElement selectedElement)
        {
            StartCoroutine(ShotAnimation(selectedElement));
        }

        private IEnumerator ShotAnimation(GridElement selectedElement)
        {
            animate = true;
        
            _shotBehaviour.Release( ref selectedElement);
            yield return new WaitForSeconds(0.8f);

            if (LevelData.Instance.PlayerState.CheckTarget() == true)
                yield break;
            
        
            if(selectedElement.HitState == GridSprites.SpriteState.missed)
                OnInputStoppedHandler?.Invoke();

            animate = false;
        }
    }
}
