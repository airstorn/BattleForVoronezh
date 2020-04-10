using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnitsDrag : MonoBehaviour, IUnitsPlacer
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactableGrid;
    private GridUnit _currentUnit;

    public void Place(GridUnit unit)
    {
    }

    public void CatchUnit()
    {
        RaycastHit hit;
        if (Physics.Raycast(_raycastCamera.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.CompareTag("Unit"))
            {
                _currentUnit = hit.collider.GetComponent<GridUnit>();
                _currentUnit.OnDrag?.Invoke();

                _interactableGrid.RemoveUnit(_currentUnit);

                StopCoroutine(DragUnit());
                StartCoroutine(DragUnit());
            }
        }
    }


    private IEnumerator DragUnit()
    {
        bool drag = true;

        while (drag == true)
        {
            _interactableGrid.PredictPlace(_currentUnit, GridObject.ElementState.vacant);

            Vector3 pos = new Vector3(
                   _raycastCamera.ScreenToWorldPoint(Input.mousePosition).x,
                   0.5f,
                   _raycastCamera.ScreenToWorldPoint(Input.mousePosition).z
                   );

            if (_currentUnit != null)
                _currentUnit.transform.position = pos;

            if (Input.GetMouseButtonUp(0))
            {
                drag = false;

                if (_interactableGrid.TryPlaceUnit(_currentUnit) == true)
                {
                    _currentUnit.SuitablePlaced = true;
                    _interactableGrid.PlaceUnit(_currentUnit);
                }
                else
                {
                    _currentUnit.SuitablePlaced = false;
                    //AddUnitToSchedule(_currentUnit);
                }
            }
            yield return null;
        }
    }

}
