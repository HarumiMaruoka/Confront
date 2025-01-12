using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public static class StateTypeExtensions
    {
        public static IState ToState(this StateType stateType, LeviathanController leviathan)
        {
            switch (stateType)
            {
                case StateType.Idle: return leviathan.StateMachine.Idle;
                case StateType.Walk: return leviathan.StateMachine.Walk;
                case StateType.Stunned: return leviathan.StateMachine.Stunned;
                case StateType.GetHit1: return leviathan.StateMachine.GetHit1;
                case StateType.GetHit2: return leviathan.StateMachine.GetHit2;
                case StateType.Die: return leviathan.StateMachine.Die;
                case StateType.Attack1: return leviathan.StateMachine.Attack1;
                case StateType.Attack2: return leviathan.StateMachine.Attack2;
                case StateType.AttackHard: return leviathan.StateMachine.AttackHard;
                case StateType.AttackSpecial: return leviathan.StateMachine.AttackSpecial;
                case StateType.Roar: return leviathan.StateMachine.Roar;
                case StateType.Block: return leviathan.StateMachine.Block;
                default: throw new ArgumentOutOfRangeException(nameof(stateType), stateType, null);
            }
        }

        public static void ChangeState(this StateType nextStateType, LeviathanController owner)
        {
            nextStateType.ToState(owner).ChangeState(owner);
        }
    }
}