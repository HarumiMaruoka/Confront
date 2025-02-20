using System;
using UnityEngine;

namespace Confront.Player
{
    public class Dead : IState
    {
        public string AnimationName => "Dead";
        public float AnimationCrossFadeTime => 0.5f;

        public void Enter(PlayerController player)
        {
            player.GameOverPanel.Show();
        }

        public void Execute(PlayerController player)
        {

        }

        public void Exit(PlayerController player)
        {
            player.GameOverPanel.Hide();
        }
    }
}