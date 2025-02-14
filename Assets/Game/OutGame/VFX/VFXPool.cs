using System;
using System.Collections.Generic;
using UnityEngine;

namespace Confront.VFXSystem
{
    public static class VFXPool
    {
        private static GameObject _vfxContainer;
        private static Dictionary<string, GameObject> _vfxParents = new Dictionary<string, GameObject>();

        private static Dictionary<string, VFX> _vfxPrefabs;
        private static Dictionary<string, Queue<VFX>> _vfxPool = new Dictionary<string, Queue<VFX>>();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _vfxPrefabs = new Dictionary<string, VFX>();
            var vfxPrefabs = Resources.LoadAll<VFX>("VFX");
            foreach (var vfx in vfxPrefabs)
            {
                _vfxPrefabs.Add(vfx.gameObject.name, vfx);
            }

            _vfxContainer = new GameObject("VFXContainer");
            GameObject.DontDestroyOnLoad(_vfxContainer);
        }

        public static VFX GetVFX(string vfxName)
        {
            if (_vfxPrefabs.TryGetValue(vfxName, out var vfxPrefab))
            {
                if (!_vfxPool.ContainsKey(vfxName)) _vfxPool.Add(vfxName, new Queue<VFX>());
                if (_vfxPool[vfxName].Count == 0)
                {
                    if (!_vfxParents.ContainsKey(vfxName))
                    {
                        _vfxParents.Add(vfxName, new GameObject(vfxName));
                        _vfxParents[vfxName].transform.SetParent(_vfxContainer.transform);
                    }

                    var vfx = GameObject.Instantiate(vfxPrefab, _vfxParents[vfxName].transform);
                    vfx.OnFinished += ReturnVFX;
                    return vfx;
                }
                else
                {
                    var vfx = _vfxPool[vfxName].Dequeue();
                    vfx.gameObject.SetActive(true);
                    return vfx;
                }
            }
            else
            {
                Debug.LogError($"VFX {vfxName} is not found.");
                return null;
            }
        }

        private static void ReturnVFX(VFX vfx)
        {
            vfx.gameObject.SetActive(false);
            if (!_vfxPool.ContainsKey(vfx.gameObject.name)) _vfxPool.Add(vfx.gameObject.name, new Queue<VFX>());
            _vfxPool[vfx.name].Enqueue(vfx);
        }
    }
}