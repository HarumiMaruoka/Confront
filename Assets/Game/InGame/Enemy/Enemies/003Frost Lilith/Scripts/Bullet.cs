using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Enemy.FrostLilith
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField]
        private GameObject _magicPrefab;
        [SerializeField]
        private ParticleSystem _particleSystem;
        [SerializeField]
        private Vector3 _targetOffset;

        private List<ParticleCollisionEvent> _collisionEvents = new List<ParticleCollisionEvent>();

        private void Awake()
        {
            if (PlayerController.Instance) Init(PlayerController.Instance.transform.position + _targetOffset);
        }

        public void Init(Vector3 target)
        {
            transform.LookAt(target);
        }

        void OnParticleCollision(GameObject other)
        {
            int collisionEventCount = _particleSystem.GetCollisionEvents(other, _collisionEvents);
            for (int i = 0; i < collisionEventCount; i++)
            {
                var instance = Instantiate(_magicPrefab, _collisionEvents[i].intersection, Quaternion.identity);
                Destroy(instance, 5f);
            }

            Destroy(gameObject);
        }
    }
}