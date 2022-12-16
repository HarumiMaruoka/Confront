
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OverLapBox
{
    [SerializeField]
    private Vector3 _offset;
    [SerializeField]
    private Vector3 _size;
    [SerializeField]
    private LayerMask _targetLayer;
    [SerializeField]
    private Transform _origin;

    public Vector3 Offset => _offset;
    public Vector3 Size => _size;
    public LayerMask TargetLayer => _targetLayer;
    public Transform Origin => _origin;

    public void Init(Transform origin)
    {
        _origin = origin;
    }

    public Collider[] GetCollider()
    {
        return Physics.OverlapBox(_origin.position + _offset, _size, Quaternion.identity, _targetLayer);
    }
}
