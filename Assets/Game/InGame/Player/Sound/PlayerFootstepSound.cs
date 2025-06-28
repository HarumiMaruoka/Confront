using Confront.Audio;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Confront.Player
{
    public class PlayerFootstepSound : MonoBehaviour
    {
        [SerializeField]
        private AudioSource _audioSource;

        [Header("Ray")]
        [SerializeField]
        private Vector3 _rayOffset = new Vector2(0, -0.5f);
        [SerializeField]
        private float _rayDistance = 2f;
        [SerializeField]
        private LayerMask _groundLayerMask;

        [Header("Footstep Sounds")]
        [SerializeField] private AudioClip[] _lightDirtFootsteps;
        [SerializeField] private AudioClip[] _mediumDirtFootsteps;
        [SerializeField] private AudioClip[] _heavyDirtFootsteps;
        [Space]
        [SerializeField] private AudioClip[] _lightGrassFootsteps;
        [SerializeField] private AudioClip[] _mediumGrassFootsteps;
        [SerializeField] private AudioClip[] _heavyGrassFootsteps;
        [Space]
        [SerializeField] private AudioClip[] _lightStoneFootsteps;
        [SerializeField] private AudioClip[] _mediumStoneFootsteps;
        [SerializeField] private AudioClip[] _heavyStoneFootsteps;
        [Space]
        [SerializeField] private AudioClip[] _waterFootsteps;

        [Header("Tags")]
        [SerializeField, TagField] private string _lightDirtTag;
        [SerializeField, TagField] private string _mediumDirtTag;
        [SerializeField, TagField] private string _heavyDirtTag;
        [Space]
        [SerializeField, TagField] private string _lightGrassTag;
        [SerializeField, TagField] private string _mediumGrassTag;
        [SerializeField, TagField] private string _heavyGrassTag;
        [Space]
        [SerializeField, TagField] private string _lightStoneTag;
        [SerializeField, TagField] private string _mediumStoneTag;
        [SerializeField, TagField] private string _heavyStoneTag;
        [Space]
        [SerializeField, TagField] private string _waterTag;

        private Dictionary<int, AudioClip[]> _soundsMap;

        private void Awake()
        {
            _soundsMap = new Dictionary<int, AudioClip[]>()
            {
                { Animator.StringToHash(_lightDirtTag), _lightDirtFootsteps},
                { Animator.StringToHash(_mediumDirtTag), _mediumDirtFootsteps},
                { Animator.StringToHash(_heavyDirtTag), _heavyDirtFootsteps},
                { Animator.StringToHash(_lightGrassTag), _lightGrassFootsteps},
                { Animator.StringToHash(_mediumGrassTag), _mediumGrassFootsteps},
                { Animator.StringToHash(_heavyGrassTag), _heavyGrassFootsteps},
                { Animator.StringToHash(_lightStoneTag), _lightStoneFootsteps},
                { Animator.StringToHash(_mediumStoneTag), _mediumStoneFootsteps},
                { Animator.StringToHash(_heavyStoneTag), _heavyStoneFootsteps},
                { Animator.StringToHash(_waterTag), _waterFootsteps},
            };
        }

        private string GroundGameObjectTag => Physics.Raycast(transform.position + _rayOffset, Vector3.down, out RaycastHit hit, _rayDistance, _groundLayerMask) ? hit.collider.tag : null;

        public AudioClip GetFootstepSound()
        {
            var groundHash = Animator.StringToHash(GroundGameObjectTag);

            if (_soundsMap.TryGetValue(groundHash, out var sounds))
            {
                return sounds[UnityEngine.Random.Range(0, sounds.Length)];
            }

            var defaultSounds = _lightDirtFootsteps;

            return defaultSounds[UnityEngine.Random.Range(0, defaultSounds.Length)];
        }

        [SerializeField]
        private float _interval = 0.1f;
        private float _timer = 0f;

        private void Update()
        {
            if (_timer >= 0)
            {
                _timer -= Time.deltaTime;
            }
        }

        public void PlayFootstepSound() // アニメーションイベントから呼び出す
        {
            if (_timer > 0) return;
            _timer = _interval;

            var sound = GetFootstepSound();
            if (sound != null)
            {
                _audioSource.volume = AudioManager.VolumeParameters.SeVolume;
                _audioSource.PlayOneShot(sound);
            }
        }

        private void OnDrawGizmos()
        {
            var hit = Physics.Raycast(transform.position + _rayOffset, Vector3.down, out RaycastHit raycastHit, _rayDistance, _groundLayerMask);
            Gizmos.color = hit ? Color.red : Color.green;
            Gizmos.DrawLine(transform.position + _rayOffset, transform.position + _rayOffset + Vector3.down * _rayDistance);
        }
    }
}