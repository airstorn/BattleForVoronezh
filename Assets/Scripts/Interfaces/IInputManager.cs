using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interfaces
{
    public delegate void OnInputStopped();

    public interface IInputHandler
    {
        void TrackInput();
        event OnInputStopped OnInputStoppedHandler;
    }
}