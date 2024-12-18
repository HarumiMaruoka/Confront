using System;
using UnityEngine;

namespace Confront.Enemy
{
    public class EnemyData : ScriptableObject
    {
        public int ID;
        public string Name;
        public string Description;
        public GameObject Prefab;
    }
}