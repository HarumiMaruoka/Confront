using Confront.Player;
using System;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    // 敵が死亡した状態を表すステートです。
    [CreateAssetMenu(fileName = "DeadState", menuName = "Enemy/Slimey/States/DeadState")]

    public class DeadState : SlimeyState
    {
        public TextAsset DropItemTable;

        public float Duration = 0.8f;

        private float _timer;

        public override string AnimationName => "Die";

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timer = 0;
            if (slimey.UniqueDropItemTable != null)
            {
                Debug.Log("ユニークドロップアイテムをドロップする");
            }
            else
            {
                Debug.Log("通常ドロップアイテムをドロップする");
            }
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                GameObject.Destroy(slimey.gameObject);
            }
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}