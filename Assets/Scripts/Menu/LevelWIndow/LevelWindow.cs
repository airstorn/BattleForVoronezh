using System.Collections;
using System.Collections.Generic;
using GameStates.Menu;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : MonoBehaviour, ILevelWindowable, IMenuPagable
{
    [SerializeField] private GameObject _object;
    [SerializeField] private Text _Name;
    [SerializeField] private Text _Description;
    [SerializeField] private Image _preview;
    [SerializeField] private Button _playButton;
    public void ShowLevelData(LevelObject level)
    {
        StartCoroutine(AnimateText(level.Description, _Description));
        _Name.text = level.LevelName;
        _preview.sprite = level.Preview;
        _playButton.onClick.RemoveAllListeners();
        _playButton.onClick.AddListener(() => Debug.Log("load scene" + level.LevelLoadIndex));
    }

    private IEnumerator AnimateText(string textData, Text asset)
    {
        float tickTime = 0.1f;
        for (int i = 0; i < textData.Length; i++)
        {
            asset.text = textData.PadRight(i);
            yield return new WaitForSeconds(tickTime);
        }
    }

    public void Show()
    {
        _object.SetActive(true);
    }

    public void Hide()
    {
        _object.SetActive(false);
    }
}
