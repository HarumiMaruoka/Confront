using System;
using UnityEngine;

namespace Confront.Utility
{
    public class HomingMissile : MonoBehaviour
    {
        private Vector3 _velocity;
        private Vector3 _position;
        private Transform _target;
        private float _period;

        private float _maxAcceleration = 10f;

        public void Initialize(Transform target, Vector3 position, Vector3 velocity, float period, float maxAcceleration = 999f)
        {
            _target = target;
            _position = position;
            _velocity = velocity;
            _period = period;
            _maxAcceleration = maxAcceleration;
        }

        private void Update()
        {
            if (_target == null)
            {
                Debug.Log("ProjectileExtensions: Target is null");
                Destroy(gameObject);
                return;
            }

            var acceleration = Vector3.zero;
            var diff = _target.position - _position;
            acceleration += (diff - _velocity * _period) * 2f / (_period * _period);
            acceleration = Vector3.ClampMagnitude(acceleration, _maxAcceleration);

            _period -= Time.deltaTime;
            if (_period <= 0)
            {
                Debug.Log("ProjectileExtensions: Target reached");
                Destroy(gameObject);
                return;
            }

            _velocity += acceleration * Time.deltaTime;
            _position += _velocity * Time.deltaTime;
            transform.position = _position;
        }
    }
}