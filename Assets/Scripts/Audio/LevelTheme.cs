using System;
using GameStates;
using UnityEngine;

namespace Audio
{
    public class LevelTheme : MonoBehaviour
    {
        public AudioClip Theme => _theme;
        [SerializeField] private AudioClip _theme;

        public void Start()
        {
            SoundsPlayer.Instance.PlayTheme(Theme);
        }
    }
}