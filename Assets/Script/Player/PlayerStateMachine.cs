using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステートマシン
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{
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
