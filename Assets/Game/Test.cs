using Confront.AttackUtility;
using Confront.Player;
using System;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private GameObject GameObject;
    [SerializeField]
    private float _actorAttackPower;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //GameObject.CreatePrimitive(PrimitiveType.Sphere).
            //AddComponent<HomingMissile>().
            //Initialize(PlayerController.Instance.transform, this.transform.position, Vector3.forward, 3f);
            //AddComponent<ProjectileMotion>().
            //Launch(this.transform.position, PlayerController.Instance.transform, UnityEngine.Random.Range(2, 5)).
            //GetComponent<Collider>().enabled = false;
            Instantiate(GameObject, this.transform.position, Quaternion.identity).
                GetComponent<ProjectileMotion>().
                Launch(_actorAttackPower, this.transform.position, PlayerController.Instance.transform, UnityEngine.Random.Range(2, 5));
        }
    }
}
