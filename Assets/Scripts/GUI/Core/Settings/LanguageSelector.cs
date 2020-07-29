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
      Ru,
      Eng
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
      
      OnLanguageChanged?.Invoke(Lang.Ru);
   }

   public void UpdateLanguage(Lang lang)
   {
      Debug.Log(lang);
      OnLanguageChanged?.Invoke(lang);
   }
}
