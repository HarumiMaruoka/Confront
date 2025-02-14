using System;
using UnityEngine;

namespace Confront.VFXSystem
{
    public static class VFXManager
    {
        public static void PlayVFX(VFX vfxPrefab, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            PlayVFX(vfxPrefab.gameObject.name, position, rotation, scale);
        }

        public static void PlayVFX(string vfxName, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            var vfx = VFXPool.GetVFX(vfxName);
            if (vfx)
            {
                vfx.transform.position = position;
                vfx.transform.rotation = rotation;
                vfx.transform.localScale = scale;
                vfx.Play();
            }
        }
    }
}