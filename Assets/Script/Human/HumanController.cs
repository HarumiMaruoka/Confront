using System;
using UniRx;
using UnityEngine;

namespace Human
{
    [System.Serializable]
    public class HumanController : MonoBehaviour
    {
        [SerializeField]
        private Talk _talkBehaviour = default;
        public Talk TalkBehaviour => _talkBehaviour;
    }
}