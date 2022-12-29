using System;
using UnityEngine;


namespace Player
{
    [System.Serializable]
    public class PlayerState07Talk : PlayerState00Base
    {
        //private Human _human = null;
        public override void Enter()
        {
            //if(_stateMachine.PlayerController.TalkChecker.Result.collider.TryGetComponent(out Human human))
            //{
            //    human?.Talk();
            //}
        }
        public override void Update()
        {
            //if(_human.IsTalkEnd)
            //{
                  // ここにステート遷移処理を記述する
            //}

        }
    }
}