using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステートマシン
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{
    private PlayerState _currentState;

    [SerializeField]
    private IdleState Idle;
    [SerializeField]
    private SatateBase Walk;
    [SerializeField]
    private SatateBase Run;
    [SerializeField]
    private SatateBase Attack1;
    [SerializeField]
    private SatateBase Attack2;
    [SerializeField]
    private SatateBase Guard;
    [SerializeField]
    private SatateBase Jamp;
    [SerializeField]
    private SatateBase Rise;
    [SerializeField]
    private SatateBase Fall;
    [SerializeField]
    private SatateBase Hide;

    void Start()
    {

    }

    void Update()
    {

    }
}



public enum PlayerState
{
    Idle,
    Walk,
    Run,
    Attack1,
    Attack2,
    Guard,
    Jamp,
    Rise,
    Fall,
    Hide,
}
