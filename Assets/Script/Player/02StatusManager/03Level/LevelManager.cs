using System;
using UnityEngine;
using UniRx;

namespace Player
{
    [System.Serializable]
    public class LevelManager
    {
        private int _currentLevel = 0;
        private int _currentEXP = 0;
        private LevelStatus _currentStatus;
        private LevelStatus[] _levelData = default;

        public int CurrentLevel => _currentLevel;
        public int CurrentEXP => _currentEXP;
        public LevelStatus[] LevelData => _levelData;
        public LevelStatus CurrentStatus => _currentStatus;

        public void LoadData(string addressableName, string saveDataFilePath)
        {

        }
    }
}