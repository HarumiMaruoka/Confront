using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Audio
{
    public class SEPlayer
    {
        private static Dictionary<string, AudioClip> _seMap;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            _seMap = new Dictionary<string, AudioClip>();
            var seList = Resources.LoadAll<AudioClip>("Audio/SE");
            foreach (var se in seList)
            {
                _seMap[se.name] = se;
            }
        }

        public void Play(Vector3 position, string audioName)
        {
            AudioSource.PlayClipAtPoint(_seMap[audioName], position, AudioManager.VolumeParameters.SeVolume);
        }
    }
}