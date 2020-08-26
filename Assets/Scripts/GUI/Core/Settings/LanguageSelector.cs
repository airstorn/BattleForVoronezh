using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
   private ILanguageSelectable[] _selectables;

   public static Action<Lang> OnLanguageChanged;

   public enum Lang
   {
      Russian,
      English
   }
   
   private void Awake()
   {
      _selectables = GetComponentsInChildren<ILanguageSelectable>();
      Debug.Log(_selectables.Length);

      foreach (var selectable in _selectables)
      {
         OnLanguageChanged += selectable.SetSelectable;
         selectable.SetButtonMethod(this);
      }

      var parsed =  (Lang)Enum.Parse(typeof(Lang), Lean.Localization.LeanLocalization.CurrentLanguage);
      OnLanguageChanged?.Invoke(parsed);
   }

   public void UpdateLanguage(Lang lang)
   {
      OnLanguageChanged?.Invoke(lang);
   }
}
