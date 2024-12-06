using System;
using UnityEngine;

namespace Confront.SaveSystem
{
    public static class SaveDataRepository
    {
        static SaveDataRepository()
        {
            SaveDataControllers = new SaveDataController[10];
            for (var i = 0; i < SaveDataControllers.Length; i++)
            {
                SaveDataControllers[i] = new SaveDataController($"SaveData {i}");
            }
        }

        public readonly static SaveDataController[] SaveDataControllers;
    }
}