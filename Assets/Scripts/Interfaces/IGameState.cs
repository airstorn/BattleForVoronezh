using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace Battle.Interfaces
{
    public interface IGameState
    {
        void Activate();
        void Deactivate();
    }

    public interface IPlayerState : IGameState
    {
        void SetInput(IInputHandler handler);
        void ResetInput();
    }

    public interface IEnemyState : IGameState
    {
    
    }
}