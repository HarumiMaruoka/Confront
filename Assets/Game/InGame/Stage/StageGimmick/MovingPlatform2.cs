using System;
using UnityEngine;

namespace Confront.StageGimmick
{
    public class MovingPlatform2 : MonoBehaviour
    {
        [SerializeField]
        private Transform[] _waypoints;
        [SerializeField]
        private float _speed = 1f;

        private int _currentWaypointIndex = 0;

        private void Start()
        {
            if (_waypoints == null || _waypoints.Length == 0)
                Debug.LogError("Waypoints are not set.");
        }

        private void Update()
        {
            if (_waypoints == null || _waypoints.Length == 0) return;

            var targetPosition = _waypoints[_currentWaypointIndex].position;
            var currentPosition = transform.position;
            var direction = (targetPosition - currentPosition).normalized;
            var sqrDistance = Vector3.Distance(targetPosition, currentPosition);
            if (sqrDistance < 0.1f)
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
            }
            else
            {
                transform.position += direction * _speed * Time.deltaTime;
            }
        }
    }
}