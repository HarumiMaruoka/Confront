using System;
using UnityEngine;

namespace Confront.Physics
{
    [CreateAssetMenu(fileName = "MovementSettings", menuName = "Confront/Physics/MovementSettings")]
    public class MovementSettings : ScriptableObject
    {
        public float _acceleration;
        public float _deceleration;
        public float _turnDeceleration;
        public float _maxSpeed;

        public float _gravity;

        public float _jumpTimeoutDelta;

        public float _abyssGravity;

        public float _slopeAcceleration;

        public float _inAirAcceleration;
        public float _inAirDeceleration;
        public float _inAirMaxSpeed;
    }
}