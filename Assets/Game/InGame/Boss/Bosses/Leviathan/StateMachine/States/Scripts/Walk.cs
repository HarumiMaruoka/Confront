using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Walk")]
    public class Walk : ScriptableObject, IState
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
