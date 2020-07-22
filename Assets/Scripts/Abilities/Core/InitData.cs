using System;
using Abilities.UI;
using UnityEngine;

namespace Abilities.Core
{
   public interface IAbilityData
   {
      
   }
   
   public struct InitData : IAbilityData
   {
      public AbilityPresenter Presenter;
      public LevelData Logic;
      
      public Action AbilityActionHandler;
   }

   public struct UseData : IAbilityData
   {
      
   }
   
   public struct VisualData : IAbilityData
   {
      public string Name;
      public int Count;
      public AbilityLevel Level;
      public Ability Reference;
   }
}
