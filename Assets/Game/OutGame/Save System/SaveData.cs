using Confront.ActionItem;
using Confront.ForgeItem;
using Confront.Player;
using Confront.Weapon;
using System;
using System.Collections.Generic;
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
        // Enemy
        public Dictionary<string, string> EnemyData = new Dictionary<string, string>();
        // Stage
        public string StageName;
        // ここに保存したいデータを追加してください。

        public void ClearEnemyData() => EnemyData.Clear();
    }

    [Serializable]
    public struct PlayerData
    {
        // PlayerController
        //   Transform
        public Vector3 Position;
        public Quaternion Rotation;
        public Direction Direction;
        //   StateMachine
        public Type PlayerStateType;
        //   MovementParameters
        public Vector2 Velocity;
        public float GrabIntervalTimer;
        public float PassThroughPlatformDisableTimer;
        //   HealthManager 
        public HealthManager HealthManager;
        //   Action Item
        public ActionItemInventory ActionItemInventory;
        public HotBar HotBar;
        //   Forge Item
        public ForgeItemInventory ForgeItemInventory;
        //   Weapon
        public WeaponInstance EquippedWeapon;
        public WeaponInventory WeaponInventory;
    }
}