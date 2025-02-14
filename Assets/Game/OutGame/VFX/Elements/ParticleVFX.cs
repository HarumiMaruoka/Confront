using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Confront.VFXSystem
{
    public class ParticleVFX : VFX
    {
        [SerializeField] protected ParticleSystem _particleSystem;
        protected float Duration => _particleSystem.main.duration;

        public override async void Play()
        {
            gameObject.SetActive(true);
            await UniTask.WaitForSeconds(Duration);
            OnFinished?.Invoke(this);
        }
    }
}
