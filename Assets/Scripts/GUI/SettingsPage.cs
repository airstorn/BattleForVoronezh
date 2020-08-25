using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace GameStates
{
    public class SettingsPage : PageBasement, IMenuPageable
    {
        [SerializeField] private Toggle _audio;
        [SerializeField] private Toggle _music;

        public override void Show()
        {
            base.Show();
            _audio.isOn = SoundsPlayer.Instance.GetChannelStatement(SoundsPlayer._soundsKey) == 0;
            _music.isOn = SoundsPlayer.Instance.GetChannelStatement(SoundsPlayer._musicKey) == 0;
        }

        public void SendArgs<T>(T args) where T : struct
        {
            throw new NotImplementedException();
        }

        public void SetAudioStatement(bool state)
        {
            SoundsPlayer.Instance.SetAudioEnabled(state);
        }

        public void SetMusicStatement(bool state)
        {
            SoundsPlayer.Instance.SetMusicEnabled(state);
        }
    }
}