using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineBrain))]
public class BlinkingCamera : MonoBehaviour, ICamMover
{
    [SerializeField] private CinemachineBlenderSettings _transitionSettings;
    [SerializeField] private CanvasGroup _blink;
    [SerializeField] private AnimationCurve _blinkCurve;
    [SerializeField] private float _elapsedTime = 1;
    [SerializeField] private CinemachineVirtualCamera _currentCamera;

    private CinemachineBrain _brain;

    private void Start()
    {
        if(_transitionSettings != null)
            GetComponent<CinemachineBrain>().m_CustomBlends = _transitionSettings;
        _blink.gameObject.SetActive(false);
        _brain = FindObjectOfType<CinemachineBrain>();
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
        if (_transitionSettings.m_CustomBlends.Any(trans =>
            trans.m_Blend.m_Style == CinemachineBlendDefinition.Style.Cut && trans.m_To == virtualCamera.name &&  trans.m_From == _currentCamera.name) || _brain.m_DefaultBlend.m_Style == CinemachineBlendDefinition.Style.Cut)
        {
            StartCoroutine(Blink(virtualCamera));
        }
        else
        {
            if(_currentCamera)
                _currentCamera.gameObject.SetActive(false);
            _currentCamera = virtualCamera;
            _currentCamera.gameObject.SetActive(true);
        }
    }

    private IEnumerator Blink(CinemachineVirtualCamera camera)
    {
        if(_currentCamera)
            _currentCamera.gameObject.SetActive(false);
        
        float time = 0;
        
        _blink.gameObject.SetActive(true);

        while (time < _elapsedTime)
        {
            _blink.alpha = _blinkCurve.Evaluate(time);
            time += Time.deltaTime;

            if (time > _elapsedTime / 2)
            {
                _currentCamera = camera;
                _currentCamera.gameObject.SetActive(true);
            }
            yield return null;
        }
        
        _blink.gameObject.SetActive(false);
    }
}
