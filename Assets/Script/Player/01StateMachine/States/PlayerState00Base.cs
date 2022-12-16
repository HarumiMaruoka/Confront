using System;
using UnityEngine;

[System.Serializable]
public class PlayerState00Base : IState
{
    protected PlayerStateMachine _stateMachine = null;
    public void Init(PlayerStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update()
    {

    }
}