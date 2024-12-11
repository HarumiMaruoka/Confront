using Confront.Item;
using Confront.Player;
using System;
using UnityEngine;

namespace Confront.SaveSystem
{
    [SerializeField]
    public class SaveFileData
    {
        public string SceneName;
        public DateTime LastPlayed;
        public Byte[] ScreenShot;
    }

    [Serializable]
    public class SaveData
    {
        // Scene
        public string SceneName;
        // PlayerController
        public PlayerData? PlayerData;
        // ここに保存したいデータを追加してください。

    }

    [Serializable]
    public struct PlayerData
    {
        // PlayerController
        //   Transform
        public Vector3 Position;
        public Quaternion Rotation;
        //   StateMachine
        public Type PlayerStateType;
        //   MovementParameters
        public Vector2 Velocity;
        public float GrabIntervalTimer;
        public float PassThroughPlatformDisableTimer;
        //   HealthManager 
        public HealthManager HealthManager;

        //   Inventory
        public ItemInventory Inventory;
        public HotBar HotBar;
    }
}