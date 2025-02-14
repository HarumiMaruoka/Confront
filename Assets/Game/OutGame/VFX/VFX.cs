using System;
using UnityEngine;

namespace Confront.VFXSystem
{
    public abstract class VFX : MonoBehaviour
    {
        public abstract void Play();
        public Action<VFX> OnFinished;
    }
}