using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Weapon
{
    [CreateAssetMenu(fileName = "WeaponSheet", menuName = "Game Data Sheets/WeaponSheet")]
    public class WeaponSheet : NexEditor.GameDataSheet.SheetBase<WeaponData>
    {
        public void Initialize()
        {
            foreach (var weapon in this)
            {
                _idToWeaponDic.Add(weapon.ID, weapon);
                _nameToWeaponDic.Add(weapon.Name, weapon);
            }
        }

        private Dictionary<int, WeaponData> _idToWeaponDic = new Dictionary<int, WeaponData>();
        private Dictionary<string, WeaponData> _nameToWeaponDic = new Dictionary<string, WeaponData>();

        public WeaponData GetWeaponData(int id)
        {
            if (_idToWeaponDic.ContainsKey(id))
            {
                return _idToWeaponDic[id];
            }
            else
            {
                Debug.LogError("WeaponSheet.GetWeapon: Weapon ID " + id + " not found.");
                return null;
            }
        }

        public WeaponData GetWeaponData(string name)
        {
            if (_nameToWeaponDic.ContainsKey(name))
            {
                return _nameToWeaponDic[name];
            }
            else
            {
                Debug.LogError("WeaponSheet.GetWeapon: Weapon name " + name + " not found.");
                return null;
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.CustomEditor(typeof(WeaponSheet))]
    public class WeaponSheetDrawer : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Window"))
            {
                WeaponSheetWindow.Init();
            }

            base.OnInspectorGUI();
        }
    }
#endif
}