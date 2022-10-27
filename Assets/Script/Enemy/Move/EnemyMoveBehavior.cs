using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMoveBehavior
{
    protected Rigidbody _rigidbody = default;

    public EnemyMoveBehavior(Rigidbody rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public abstract void Move();
}