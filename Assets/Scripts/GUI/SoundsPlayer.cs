using System;
using System.Collections.Generic;
using DigitalRuby.SoundManagerNamespace;
using GUI.Core;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using Utils;

namespace GameStates
{
    public class SoundsPlayer : Singleton<SoundsPlayer>
    {
        [Serializable]
        private struct SoundTypeContainer
        {
            public AudioClip Clip;
            public SoundType Type;
            public AudioMixerGroup Group;
            [HideInInspector] public AudioSource Source;
        }
        
        [SerializeField] private SoundTypeContainer[] _pulledData;
        
        private Dictionary<SoundType, SoundTypeContainer> _storage = new Dictionary<SoundType, SoundTypeContainer>();

        public const string _soundsKey = "soundsVolume";
        public  const string _musicKey = "musicVolume";

        [Header("AudioGroups")]
        [SerializeField] private AudioMixerGroup _soundsGroup;
        [SerializeField] private AudioMixerGroup _musicGroup;

        protected override void Awake()
        {
            base.Awake();
            PullAudioSources();
        }

        private void Start()
        {
            _soundsGroup.audioMixer.SetFloat(_soundsKey, GetChannelStatement(_soundsKey));
            _musicGroup.audioMixer.SetFloat(_musicKey, GetChannelStatement(_musicKey));
        }

        public void SetAudioEnabled(bool enabled)
        {
            PlayerPrefs.SetInt(_soundsKey, enabled == true ? 0 : -80);
            _soundsGroup.audioMixer.SetFloat(_soundsKey, GetChannelStatement(_soundsKey));
        }
        
        public void SetMusicEnabled(bool enabled)
        {
            PlayerPrefs.SetInt(_musicKey, enabled == true ? 0 : -80);
            _musicGroup.audioMixer.SetFloat(_musicKey, GetChannelStatement(_musicKey));
        }

        public int GetChannelStatement(string path)
        {
            return PlayerPrefs.GetInt(path, 0);
        }

        private void PullAudioSources()
        {
            for (int i = 0; i < _pulledData.Length; i++)
            {
                var source = _pulledData[i].Source = gameObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = _pulledData[i].Group;
                source.clip = _pulledData[i].Clip; 
                _storage.Add(_pulledData[i].Type, _pulledData[i]);
            }
        }

        public void PlaySound(SoundType sound)
        {
            PlayAudio(_storage[sound]);
        }

        private void PlayAudio(SoundTypeContainer data)
        {
            data.Source.Play();
        }
    }
}
