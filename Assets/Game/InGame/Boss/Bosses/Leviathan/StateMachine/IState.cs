using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public interface IState
    {
        string AnimationName { get; }
        float AnimationClossFadeTime { get => 0.1f; }
        void Enter(LeviathanController owner);
        void Execute(LeviathanController owner);
        void Exit(LeviathanController owner);
    }
}