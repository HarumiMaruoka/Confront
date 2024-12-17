using Confront.Player;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public abstract class Projectile : MonoBehaviour
    {
        public abstract void Initialize(PlayerController player);
    }
}