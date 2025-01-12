using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public static class IStateExtensions
    {
        public static void ChangeState(this IState nextState, LeviathanController owner)
        {
            owner.StateMachine.ChangeState(nextState);
        }
    }
}