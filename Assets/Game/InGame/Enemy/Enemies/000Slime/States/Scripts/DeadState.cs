using Confront.Player;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Confront.Enemy.Slimey
{
    // 敵が死亡した状態を表すステートです。
    [CreateAssetMenu(fileName = "DeadState", menuName = "ConfrontSO/Enemy/Slimey/States/DeadState")]

    public class DeadState : SlimeyState
    {
        public TextAsset DropItemTable;

        public float Duration = 0.8f;

        private float _timer;

        public override string AnimationName => "Die";

        public override void Enter(PlayerController player, SlimeyController slimey)
        {
            _timer = 0;

            slimey.DropItem(player, slimey.transform.position).Forget();
        }

        public override void Execute(PlayerController player, SlimeyController slimey)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                slimey.gameObject.SetActive(false);
            }
        }

        public override void Exit(PlayerController player, SlimeyController slimey)
        {

        }
    }
}