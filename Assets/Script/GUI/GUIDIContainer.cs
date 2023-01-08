using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGUI
{
    /// <summary>
    /// 各GUIの参照をまとめて持つシングルトン
    /// </summary>
    public class GUIDIContainer
    {
        #region Singleton
        private static GUIDIContainer _instance = new GUIDIContainer();
        public static GUIDIContainer Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.LogError($"Error! Please correct!");
                }
                return _instance;
            }
        }
        private GUIDIContainer() { }
        #endregion


    }
}