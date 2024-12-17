using Confront.Player;
using System;
using UnityEngine;

namespace Confront.AttackUtility
{
    public abstract class ChargedProjectile : MonoBehaviour
    {
        public abstract void Initialize(PlayerController player, float chargeAmount);
    }
}