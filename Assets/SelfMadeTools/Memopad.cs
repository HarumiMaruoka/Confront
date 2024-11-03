using System;
using UnityEngine;

namespace SelfMadeTools
{
    public class Memopad : MonoBehaviour
    {
        [TextArea(2, 100)]
        [SerializeField] private string _text;

    }
}