using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/AttackSelector")]
    public class AttackSelector : ScriptableObject
    {
        public float[] _distanceThresholds;
        public IState[] _states;
    }
}