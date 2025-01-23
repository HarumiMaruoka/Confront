using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Player
{
    public class PlayerFootstepSound : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [SerializeField]
        private AudioClip[] _lightDirtFootsteps, _mediumDirtFootsteps, _heavyDirtFootsteps;
        [SerializeField]
        private AudioClip[] _lightGrassFootsteps, _mediumGrassFootsteps, _heavyGrassFootsteps;
        [SerializeField]
        private AudioClip[] _lightStoneFootsteps, _mediumStoneFootsteps, _heavyStoneFootsteps;
        [SerializeField]
        private AudioClip[] _waterFootsteps;

        [SerializeField]
        private string _lightDirtTag, _mediumDirtTag, _heavyDirtTag;
        [SerializeField]
        private string _lightGrassTag, _mediumGrassTag, _heavyGrassTag;
        [SerializeField]
        private string _lightStoneTag, _mediumStoneTag, _heavyStoneTag;
        [SerializeField]
        private string _waterTag;

        private Dictionary<int, AudioClip[]> _soundsMap;

        private void Awake()
        {
            _soundsMap = new Dictionary<int, AudioClip[]>
            {
                { Animator.StringToHash("LightDirt"), _lightDirtFootsteps},
                { Animator.StringToHash("MediumDirt"), _mediumDirtFootsteps},
                { Animator.StringToHash("HeavyDirt"), _heavyDirtFootsteps},
                { Animator.StringToHash("LightGrass"), _lightGrassFootsteps},
                { Animator.StringToHash("MediumGrass"), _mediumGrassFootsteps},
                { Animator.StringToHash("HeavyGrass"), _heavyGrassFootsteps},
                { Animator.StringToHash("LightStone"), _lightStoneFootsteps},
                { Animator.StringToHash("MediumStone"), _mediumStoneFootsteps},
                { Animator.StringToHash("HeavyStone"), _heavyStoneFootsteps},
                { Animator.StringToHash("Water"), _waterFootsteps},
            };
        }

        private GameObject _groundGameObject => Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 2f) ? hit.collider.gameObject : null;

        public AudioClip GetFootstepSound()
        {
            var groundTag = _groundGameObject.tag;
            var groundHash = Animator.StringToHash(groundTag);

            if (_soundsMap.TryGetValue(groundHash, out var sounds))
            {
                return sounds[UnityEngine.Random.Range(0, sounds.Length)];
            }

            return null;
        }

        public void PlayFootstepSound() // Animation Event から呼び出す。
        {
            var sound = GetFootstepSound();
            if (sound != null)
            {
                _audioSource.PlayOneShot(sound);
            }
        }
    }
}