using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState07Talk : PlayerState00Base
    {
        //private TalkHuman _human = null;
        public override void Enter()
        {
            //if(_stateMachine.PlayerController.TalkChecker.Result.collider.TryGetComponent(out Human human))
            //{
            //    human?.Talk();
            //}
        }
        public override void Update()
        {
            // 何かの影響でTalkHumanとの距離が離れすぎた場合、
            // あるいは、会話アルゴリズムが終了した場合、ステートを遷移する。
            // Vector3 a = (_stateMachine.PlayerController.transform.position- _human.transform.position).sqrMagnitude;
            
            //if(_human.IsTalkEnd)
            //{
                  // ここにステート遷移処理を記述する
            //}

        }
    }
}