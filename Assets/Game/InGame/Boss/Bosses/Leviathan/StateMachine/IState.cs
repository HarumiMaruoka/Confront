using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    public interface IState
    {
        string AnimationName { get; }
        void Enter(LeviathanController owner);
        void Execute(LeviathanController owner);
        void Exit(LeviathanController owner);
    }
}