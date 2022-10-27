using System;
using UnityEngine;

/// <summary>
/// ³–Ê‚ÉRay‚ğ•ú‚Â’ŠÛƒNƒ‰ƒX
/// </summary>
[System.Serializable]
public abstract class FireRaycastFrontBehavior : IFireBehavior
{
    public abstract void OnFire(Vector3 originPos, Vector3 dir,
    float maxDistance, RaycastHit hitInfo);
}