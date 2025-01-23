using System;
using UnityEngine;
using UnityEngine.UI;

namespace Confront.Audio
{
    public class VolumeAdjuster : MonoBehaviour
    {
        [SerializeField]
        private Slider _masterVolume;
        [SerializeField]
        private Slider _bgmVolume;
        [SerializeField]
        private Slider _seVolume;

        private VolumeParameters VolumeParameters => AudioManager.VolumeParameters;

        private void OnMasterVolumeChanged(float value) => VolumeParameters.MasterVolume = value;
        private void OnBgmVolumeChanged(float value) => VolumeParameters.BgmVolume = value;
        private void OnSeVolumeChanged(float value) => VolumeParameters.SeVolume = value;

        private void Awake()
        {
            _masterVolume.minValue = 0;
            _bgmVolume.minValue = 0;
            _seVolume.minValue = 0;

            _masterVolume.maxValue = 1;
            _bgmVolume.maxValue = 1;
            _seVolume.maxValue = 1;

            _masterVolume.value = VolumeParameters.MasterVolume;
            _bgmVolume.value = VolumeParameters.BgmVolume;
            _seVolume.value = VolumeParameters.SeVolume;

            _masterVolume.onValueChanged.AddListener(OnMasterVolumeChanged);
            _bgmVolume.onValueChanged.AddListener(OnBgmVolumeChanged);
            _seVolume.onValueChanged.AddListener(OnSeVolumeChanged);
        }

        private void OnDisable()
        {
            VolumeParameters.Save();
        }
    }
}