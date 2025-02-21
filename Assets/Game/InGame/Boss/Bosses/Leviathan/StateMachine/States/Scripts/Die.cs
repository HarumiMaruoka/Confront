using INab.Dissolve;
using System;
using UnityEngine;

namespace Confront.Boss.Leviathan
{
    [CreateAssetMenu(menuName = "ConfrontSO/Boss/Leviathan/Die")]
    public class Die : ScriptableObject, IState
    {
        private float _timer = 0f;
        public Dissolver Dissolver;

        private float Duration => Dissolver.duration;

        public string AnimationName => "Die";

        public void Enter(LeviathanController owner)
        {
            _timer = 0f;
            Dissolver.Dissolve();
            SetCollidersActive(owner, false);
        }

        public void Execute(LeviathanController owner)
        {
            _timer += Time.deltaTime;
            if (_timer >= Duration)
            {
                owner.gameObject.SetActive(false);
                ThankYouMessagePanel.Instance.Show();
            }
        }

        public void Exit(LeviathanController owner)
        {
            SetCollidersActive(owner, true);
        }

        private void SetCollidersActive(LeviathanController owner, bool isActive)
        {
            foreach (var part in owner.BossParts)
            {
                part.gameObject.SetActive(isActive);
            }
        }
    }
}