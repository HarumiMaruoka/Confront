using Confront.Audio;
using System;
using UnityEngine;

namespace Confront.Title
{
    public class TitleController : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _bgm;

        private void Start()
        {
            AudioManager.PlayBGM(_bgm);
        }
    }
}