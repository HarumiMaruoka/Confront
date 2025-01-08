using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.Weapon
{
    public class WeaponActivator : MonoBehaviour
    {
        [SerializeField]
        private WeaponActivationData[] _weaponActivationDatas;

        private WeaponActivationData _current = null;

        private Dictionary<int, WeaponActivationData> _weaponActivationDataDic = new Dictionary<int, WeaponActivationData>();

        private void Awake()
        {
            for (int i = 0; i < _weaponActivationDatas.Length; i++)
            {
                if (_weaponActivationDataDic.ContainsKey(_weaponActivationDatas[i].ID))
                {
                    Debug.LogError("WeaponActivationData ID is duplicated. ID : " + _weaponActivationDatas[i].ID);
                    continue;
                }
                _weaponActivationDataDic.Add(_weaponActivationDatas[i].ID, _weaponActivationDatas[i]);
            }
        }

        // Test
        private void Update()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.F3))
            {
                ActivateWeapon(0);
            }
            if (UnityEngine.Input.GetKeyDown(KeyCode.F4))
            {
                ActivateWeapon(1);
            }
        }

        public void ActivateWeapon(int id)
        {
            if (_current != null && _current.ID == id) return;

            _current?.Deactivation();

            if (_weaponActivationDataDic.ContainsKey(id))
            {
                _current = _weaponActivationDataDic[id];
                _current.Activation();
            }
            else
            {
                Debug.LogError("WeaponActivationData is not exist. ID : " + id);
            }
        }

        public void DeactivateWeapon()
        {
            if (_current != null)
            {
                _current.Deactivation();
                _current = null;
            }
        }

        [Serializable]
        public class WeaponActivationData
        {
            public int ID;
            public GameObject Prefab;
            public Transform Parent;
            [HideInInspector]
            public GameObject Instance;

            public void Activation()
            {
                if (Instance == null)
                {
                    Instance = GameObject.Instantiate(Prefab, Parent);
                }
                else
                {
                    Instance.SetActive(true);
                }
            }

            public void Deactivation()
            {
                if (Instance != null)
                {
                    Instance.SetActive(false);
                }
            }
        }
    }
}