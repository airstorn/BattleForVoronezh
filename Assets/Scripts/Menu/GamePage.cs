using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStates
{
    public class GamePage : PageBasement, IMenuPagable
    {
        private void Awake()
        {
            _pageObject = gameObject;
        }
    }
}
