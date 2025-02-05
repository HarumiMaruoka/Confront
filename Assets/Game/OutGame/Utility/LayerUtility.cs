using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public class LayerUtility
    {
        private static LayerMask _playerLayerMask;
        private static LayerMask _groundLayerMask;
        private static LayerMask _enemyLayerMask;
        private static LayerMask _passThroughPlatformLayerMask;

        public static LayerMask PlayerLayerMask
        {
            get
            {
                if (_playerLayerMask == 0) InitializeLayerMask();
                return _playerLayerMask;
            }
        }

        public static LayerMask GroundLayerMask
        {
            get
            {
                if (_groundLayerMask == 0) InitializeLayerMask();
                return _groundLayerMask;
            }
        }

        public static LayerMask EnemyLayerMask
        {
            get
            {
                if (_enemyLayerMask == 0) InitializeLayerMask();
                return _enemyLayerMask;
            }
        }

        public static LayerMask PassThroughPlatformLayerMask
        {
            get
            {
                if (_passThroughPlatformLayerMask == 0) InitializeLayerMask();
                return _passThroughPlatformLayerMask;
            }
        }


        [RuntimeInitializeOnLoadMethod]
        private static void InitializeLayerMask()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
            _groundLayerMask = LayerMask.GetMask("Ground");
            _enemyLayerMask = LayerMask.GetMask("Enemy");
            _passThroughPlatformLayerMask = LayerMask.GetMask("PassThroughPlatform");
        }
    }
}