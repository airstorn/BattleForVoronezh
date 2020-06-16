using System;
using System.Collections;
using System.Collections.Generic;
using GameStates;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWindow : PageBasement, ILevelWindowable, IMenuPagable
{
    [SerializeField] private Text _Name;
    [SerializeField] private TMP_Text _description;
    [SerializeField] private Image _preview;
    [SerializeField] private Button _playButton;
    public void ShowLevelData(LevelObject level)
    {
        _playButton.gameObject.SetActive(false);
        _description.pageToDisplay = 1;
        _description.text = level.Description;
        _Name.text = level.LevelName;
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

    private void LoadLevel(int levelId)
    {
        SceneManager.LoadScene(levelId);
    }

    private void ShowPlayButton()
    {
        _playButton.gameObject.SetActive(true);
    }
}
