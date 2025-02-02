using Confront.AttackUtility;
using Confront.Utility;
using System;
using UnityEngine;

namespace Confront.Enemy.Muscomorph
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private float _radius = 0.5f;
        [SerializeField]
        private float _minDuration = 2f;
        [SerializeField]
        private float _maxDuration = 5f;

        [Header("Damage")]
        [SerializeField]
        private float _baseDamage = 10;
        [SerializeField]
        private float _damageFactor = 1f;

        private ProjectileMotion _projectileMotion;

        public void Initialize()
        {
            if (_projectileMotion == null)
            {
                _projectileMotion = this.gameObject.AddComponent<ProjectileMotion>();
                _projectileMotion.OnComplete += OnComplete;
            }

            var duration = UnityEngine.Random.Range(_minDuration, _maxDuration);
            _projectileMotion.Launch(this.transform.position, Player.PlayerController.Instance.transform, duration);
            _lastPos = transform.position;
        }

        private Vector3 _lastPos;

        private void Update()
        {
            var playerLayer = LayerUtility.PlayerLayerMask;
            var groundLayer = LayerUtility.GroundLayerMask;

            _lastPos = transform.position;
        }

        private void OnComplete(ProjectileMotion motion)
        {
            gameObject.SetActive(false);
        }
    }
}