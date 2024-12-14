using Confront.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.AttackUtility
{
    public class Projectile : MonoBehaviour
    {
        private HashSet<int> _alreadyHits = new HashSet<int>(); // すでにヒットしたゲームオブジェクトのID

        public void Initialize(PlayerController player, float chargeAmount)
        {

        }

        private void Update()
        {

        }
    }
}