using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Attack2")]
    public class Attack2 : ScriptableObject, IState
    {
        public string AnimationName => "";

        public void Enter(LeviathanController owner)
        {

        }

        public void Execute(LeviathanController owner)
        {

        }

        public void Exit(LeviathanController owner)
        {

        }
    }
}