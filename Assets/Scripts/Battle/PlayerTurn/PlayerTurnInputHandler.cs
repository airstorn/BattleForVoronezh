using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnInputHandler : MonoBehaviour, IInputHandler
{
    [SerializeField] private Camera _raycastCamera;
    [SerializeField] private GridObject _interactionGrid;
    [SerializeField] private Transform _pointVisualiser;
    [SerializeField] private LayerMask _raycastIgnore;

    public void TrackInput()
    {
        var ray = _raycastCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {

            if (Physics.Raycast(ray, out hit, 1000, _raycastIgnore))
            {
                if (hit.collider != null)
                {
                    Vector3Int roundedPos = Vector3Int.RoundToInt(hit.point);
                    _pointVisualiser.position = roundedPos;
                    HighlightPoint(roundedPos);
                }
            }
        }
    }

    private void HighlightPoint(Vector3Int pos)
    {
        _interactionGrid.UpdateGridEngagements();
        _interactionGrid.GetVacantElement(pos).SetElementEngagement(GridObject.ElementState.vacant);
    }
}
