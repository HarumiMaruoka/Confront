using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorAttackState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.LogError("エラー! AttackIDの指定が間違っています！修正してください！");
    }
}