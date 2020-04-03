using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTurns : MonoBehaviour
{
    public Cinemachine.CinemachineVirtualCamera _playerCam;
    public Cinemachine.CinemachineVirtualCamera _enemyCam;
    public Cinemachine.CinemachineVirtualCamera _placerCam;

    public void ToPlacerCam()
    {
        _playerCam.gameObject.SetActive(false);
        _enemyCam.gameObject.SetActive(false);
        _placerCam.gameObject.SetActive(true);
    }
    
    public void ToPlayerCam()
    {
        _playerCam.gameObject.SetActive(true);
        _enemyCam.gameObject.SetActive(false);
        _placerCam.gameObject.SetActive(false);
    }

    public void ToEnemyCam()
    {
        _playerCam.gameObject.SetActive(false);
        _enemyCam.gameObject.SetActive(true);
        _placerCam.gameObject.SetActive(false);
    }
}
