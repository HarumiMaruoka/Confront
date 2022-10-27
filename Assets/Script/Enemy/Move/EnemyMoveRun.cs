using UnityEngine;

/// <summary>
/// 敵の移動 : 走り
/// </summary>
public class EnemyMoveRun : EnemyMoveBehavior
{
    private Transform _transform;
    private float _moveSpeed;
    private Vector3 _targetPos;

    public EnemyMoveRun(Rigidbody rigidbody,
        Transform transform, float moveSpeed, Vector3 targetPos) :
    base(rigidbody)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
        _targetPos = targetPos;
    }

    public override void Move()
    {
        var dir = _targetPos - _transform.position;
        _rigidbody.velocity = dir * _moveSpeed;
    }
}