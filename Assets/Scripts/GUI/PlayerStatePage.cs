using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace GameStates
{
    public class PlayerStatePage : PageBasement, IMenuPageable
    {
        private void Awake()
        {
            _pageObject = gameObject;
        }

        public void SendArgs<T>(T args) where T : struct
        {
            throw new NotImplementedException();
        }

        public override void Hide()
        {
            base.Hide();
        }
    }
}
