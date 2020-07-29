using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ILanguageSelectable
{
    void SetSelectable(LanguageSelector.Lang language);
    void SetButtonMethod(LanguageSelector selector);
}
