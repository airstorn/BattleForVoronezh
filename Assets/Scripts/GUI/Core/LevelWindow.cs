using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWindow : PageBasement, ILevelWindowable, IMenuPageable
{
    [SerializeField] private Text _Name;
    [SerializeField] private TMP_Text _objective;
    [SerializeField] private GameObject _descriptionBg;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _preview;
    [SerializeField] private Button _playButton;

    private int _page = 0;
    
    private int _objectivePageCount;
    private int _descriptionPageCount;
    private int _totalPageCount => _objectivePageCount + _descriptionPageCount;
    
    public void ShowLevelData(LevelObject level)
    {
        _playButton.gameObject.SetActive(false);
        _description.pageToDisplay = 1;
        _description.text = Lean.Localization.LeanLocalization.GetTranslationText(level.Description);
        _objective.text = Lean.Localization.LeanLocalization.GetTranslationText(level.Objective);

        _Name.text = Lean.Localization.LeanLocalization.GetTranslationText(level.LevelName);

        _page = 1;    
        
        _preview.sprite = level.Preview;
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => LoadLevel(level.LevelLoadIndex));

        StartCoroutine(CheckPagesAfterFrame());
    }

    private IEnumerator CheckPagesAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        
        _objectivePageCount = _objective.textInfo.pageCount;
        _descriptionPageCount = _description.textInfo.pageCount;

        CheckLastPage();
        SetPage(0);
    }

    public void SetPage(int moveOffset)
    { 
        _page = Mathf.Clamp(_page + moveOffset, 1, _descriptionPageCount + _objectivePageCount);

        if (_page >= 1 && _page <= _descriptionPageCount)
        {
            _descriptionBg.gameObject.SetActive(true);
            _objective.gameObject.SetActive(false);

            _description.pageToDisplay = _page;
        }
        else
        {
            _objective.gameObject.SetActive(true);
            _descriptionBg.gameObject.SetActive(false);

            _objective.pageToDisplay = _page - _descriptionPageCount;
        }

        CheckLastPage();


        // if (offset >= 1 && offset <= _description.textInfo.pageCount)
        // {
        //     _description.pageToDisplay = offset;
        //     CheckLastPage();
        // }
    }

    private void CheckLastPage()
    {
        if (_page == _totalPageCount)
        {
            ShowPlayButton();
        }
    }

    public void CloseButton()
    {
        _pageObject.SetActive(false);
    }

    private void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    private void ShowPlayButton()
    {
        _playButton.gameObject.SetActive(true);
    }

    public void SendArgs<T>(T args) where T : struct
    {
        throw new NotImplementedException();
    }
}
