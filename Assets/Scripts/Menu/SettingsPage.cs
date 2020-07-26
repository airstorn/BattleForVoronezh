using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace GameStates
{
    public class SettingsPage : PageBasement, IMenuPageable
    {
        public void SendArgs<T>(T args) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}