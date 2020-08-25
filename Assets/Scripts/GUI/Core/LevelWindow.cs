﻿using System;
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
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _preview;
    [SerializeField] private Button _playButton;
    public void ShowLevelData(LevelObject level)
    {
        _playButton.gameObject.SetActive(false);
        _description.pageToDisplay = 1;
        _description.text = Lean.Localization.LeanLocalization.GetTranslationText(level.Description);
        _Name.text = Lean.Localization.LeanLocalization.GetTranslationText(level.LevelName);
        _preview.sprite = level.Preview;
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => LoadLevel(level.LevelLoadIndex));

        StartCoroutine(CheckPagesAfterFrame());
    }

    private IEnumerator CheckPagesAfterFrame()
    {
        yield return new WaitForEndOfFrame();
        CheckLastPage();
    }

    public void SetPage(int moveOffset)
    {
        int offset = _description.pageToDisplay + moveOffset;
        if (offset >= 1 && offset <= _description.textInfo.pageCount)
        {
            _description.pageToDisplay = offset;
            CheckLastPage();
        }
    }

    private void CheckLastPage()
    {
        if (_description.pageToDisplay == 1 && _description.textInfo.pageCount == 0)
        {
            ShowPlayButton();
        }
        else if (_description.pageToDisplay == _description.textInfo.pageCount)
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
