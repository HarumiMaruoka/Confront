using Confront.Player;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.Enemy.VampireBat
{
    [CreateAssetMenu(fileName = "Die", menuName = "ConfrontSO/Enemy/VampireBat/States/Die")]
    public class Die : VampireBatState
    {
        public TextAsset DropItemTable;

        public float Duration = 0.8f;

        private float _timer;

        public override string AnimationName => "Die";

        public override void Enter(PlayerController player, VampireBatController vampireBat)
        {
            _timer = 0;
            vampireBat.DropItem(vampireBat.transform.position).Forget();
        }

        public override void Execute(PlayerController player, VampireBatController vampireBat)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                vampireBat.gameObject.SetActive(false);
            }
        }

        public override void Exit(PlayerController player, VampireBatController vampireBat)
        {

        }
    }
}