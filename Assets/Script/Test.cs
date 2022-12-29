using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]
    private Helper.OverLapBox _overLapBox;
    void Start()
    {
        _overLapBox.Init(transform);
    }

    void Update()
    {
        if (_overLapBox.IsHit())
        {
            Debug.LogError($"ƒqƒbƒg‚µ‚Ä‚Ü‚·");
        }
    }
    private void OnDrawGizmos()
    {
        _overLapBox.OnDrawGizmos(transform);
    }
}
