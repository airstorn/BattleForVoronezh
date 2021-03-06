﻿using System;
using Abilities.UI;
using Core;
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

   public struct ShopData : IAbilityData
   {
      public VisualData Visual;
      public int GradePrice;
      public int BuyPrice;
   }

   public struct ExtraMovesData : IAbilityData
   {
   }
   
   public struct VisualData : IAbilityData
   {
      public AbilityObject Data;
      public int Count;
      public AbilityLevel Level;
      public Ability Reference;
   }
}
