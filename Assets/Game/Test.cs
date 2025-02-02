using Confront.Player;
using Confront.Utility;
using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameObject.CreatePrimitive(PrimitiveType.Sphere).
                //AddComponent<HomingMissile>().
                //Initialize(PlayerController.Instance.transform, this.transform.position, Vector3.forward, 3f);
                AddComponent<ProjectileMotion>().
                Launch(this.transform.position, PlayerController.Instance.transform, UnityEngine.Random.Range(2, 5)).
                GetComponent<Collider>().enabled = false;
        }
    }
}
