using System;
using Core;

namespace GameStates
{
    public class EnemyStatePage : PageBasement, IMenuPageable
    {
        private void Awake()
        {
            _pageObject = gameObject;
        }

        public void SendArgs<T>(T args) where T : struct
        {
            throw new NotImplementedException();
        }
    }
}