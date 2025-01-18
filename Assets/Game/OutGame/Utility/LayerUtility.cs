using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public class LayerUtility
    {
        private static LayerMask _playerLayerMask;

        public static LayerMask PlayerLayerMask
        {
            get
            {
                if (_playerLayerMask == 0) InitializeLayerMask();
                return _playerLayerMask;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeLayerMask()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
        }
    }
}