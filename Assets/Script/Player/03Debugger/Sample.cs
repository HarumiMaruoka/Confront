using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    [InputName, SerializeField]
    private string aaa;
    [TagName, SerializeField]
    private string bbb;
    [SceneName, SerializeField]
    private string ccc;
    [AnimationParameter, SerializeField]
    private string ddd;
}