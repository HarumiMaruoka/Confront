using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Confront.Utility
{
    [DefaultExecutionOrder(-100)]
    public class PostExposureController : MonoBehaviour
    {
        public static PostExposureController Instance { get; private set; }
        private void Awake() => Instance = this;

        [SerializeField] private Volume _volume;

        private ColorAdjustments _colorAdjustments;
        private ColorAdjustments ColorAdjustments
        {
            get
            {
                if (_colorAdjustments == null) _volume.profile.TryGet(out _colorAdjustments);
                return _colorAdjustments;
            }
        }

        public void SetPostExposure(float value)
        {
            if (ColorAdjustments != null) ColorAdjustments.postExposure.value = value;
        }
    }
}