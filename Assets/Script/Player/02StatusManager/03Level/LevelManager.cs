using System;
using UnityEngine;
using UniRx;

namespace Player
{
    [System.Serializable]
    public class LevelManager
    {


        private LevelStatus _totalStatus;
        /// <summary> レベルのステータスを表す値 </summary>
        public LevelStatus TotalStatus=> _totalStatus;

    }
}