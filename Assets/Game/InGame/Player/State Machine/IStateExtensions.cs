using System;
using UnityEngine;

namespace Confront.Player
{
    public static class IStateExtensions
    {
        public static void TransitionToDefaultState(this IState state, PlayerController player)
        {
            var sensorResult = player.Sensor.CalculateGroundState(player);

            if (sensorResult.GroundType == state.ToGroundType()) return;

            switch (sensorResult.GroundType)
            {
                case GroundType.Ground: player.StateMachine.ChangeState<Grounded>(); break;
                case GroundType.Abyss: player.StateMachine.ChangeState<Abyss>(); break;
                case GroundType.SteepSlope: player.StateMachine.ChangeState<SteepSlope>(); break;
                case GroundType.InAir: player.StateMachine.ChangeState<InAir>(); break;
            }
        }

        public static GroundType ToGroundType(this IState state)
        {
            switch (state)
            {
                case Grounded _: return GroundType.Ground;
                case Abyss _: return GroundType.Abyss;
                case SteepSlope _: return GroundType.SteepSlope;
                case InAir _: return GroundType.InAir;
                default: return GroundType.None;
            }
        }
    }
}