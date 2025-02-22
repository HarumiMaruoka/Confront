using System;
using UnityEngine;

namespace Confront.Audio
{
    public class VolumeParameters
    {
        private float _masterVolume = 0.5f;
        private float _bgmVolume = 0.5f;
        private float _seVolume = 0.3f;

        public event Action<float> OnBgmVolumeChanged;
        public event Action<float> OnSeVolumeChanged;

        public VolumeParameters()
        {
            Load();
        }

        public float MasterVolume
        {
            get => _masterVolume;
            set
            {
                _masterVolume = Mathf.Clamp01(value);
                OnBgmVolumeChanged?.Invoke(_bgmVolume * _masterVolume);
                OnSeVolumeChanged?.Invoke(_seVolume * _masterVolume);
            }
        }

        public float BgmVolume
        {
            get => _bgmVolume * _masterVolume;
            set
            {
                _bgmVolume = Mathf.Clamp01(value);
                OnBgmVolumeChanged?.Invoke(_bgmVolume * _masterVolume);
            }
        }

        public float SeVolume
        {
            get => _seVolume * _masterVolume;
            set
            {
                _seVolume = Mathf.Clamp01(value);
                OnSeVolumeChanged?.Invoke(_seVolume * _masterVolume);
            }
        }

        public void Save()
        {
            PlayerPrefs.SetFloat("MasterVolume", _masterVolume);
            PlayerPrefs.SetFloat("BgmVolume", _bgmVolume);
            PlayerPrefs.SetFloat("SeVolume", _seVolume);
        }

        public void Load()
        {
            _masterVolume = PlayerPrefs.GetFloat("MasterVolume", _masterVolume);
            _bgmVolume = PlayerPrefs.GetFloat("BgmVolume", _bgmVolume);
            _seVolume = PlayerPrefs.GetFloat("SeVolume", _seVolume);
        }
    }
}