using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour, ILanguageSelectable
{
    [SerializeField] private GameObject _checkbox;
    [SerializeField] private LanguageSelector.Lang _ownLanguage;
    public void SetSelectable(LanguageSelector.Lang language)
    {
        _checkbox.SetActive(language == _ownLanguage);
    }

    public void SetButtonMethod(LanguageSelector selector)
    {
        GetComponent<Button>().onClick.AddListener(delegate { selector.UpdateLanguage(_ownLanguage); });;
    }
}
