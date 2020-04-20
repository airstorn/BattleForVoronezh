using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTurns : MonoBehaviour, ICamMover
{ 
    [SerializeField] private Cinemachine.CinemachineVirtualCamera _currentCamera;


    private void Start()
    {
        DisableAllCams();
        _currentCamera.gameObject.SetActive(true);
    }

    private void DisableAllCams()
    {
        var cams = FindObjectsOfType<CinemachineVirtualCamera>();

        foreach (var cinemachineVirtualCamera in cams)
        {
            cinemachineVirtualCamera.gameObject.SetActive(false);
        }
    }

    public void ToCamera(CinemachineVirtualCamera virtualCamera)
    {
        if(_currentCamera)
            _currentCamera.gameObject.SetActive(false);
        _currentCamera = virtualCamera;
        _currentCamera.gameObject.SetActive(true);
    }
}
