using Confront.Enemy;
using System;
using UnityEngine;

namespace Confront.Utility
{
    public class LayerUtility
    {
        private static LayerMask _playerLayerMask;
        private static LayerMask _playerDodgeLayerMask;
        private static LayerMask _groundLayerMask;
        private static LayerMask _enemyLayerMask;
        private static LayerMask _noCollisionEnemy;
        private static LayerMask _platformEnemy;
        private static LayerMask _passThroughPlatformLayerMask;
        private static LayerMask _grabbablePointLayerMask;

        public static LayerMask PlayerLayerMask
        {
            get
            {
                if (_playerLayerMask == 0) InitializeLayerMask();
                return _playerLayerMask;
            }
        }

        public static LayerMask PlayerDodgeLayerMask
        {
            get
            {
                if (_playerDodgeLayerMask == 0) InitializeLayerMask();
                return _playerDodgeLayerMask;
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

        public static LayerMask NoCollisionEnemy
        {
            get
            {
                if (_noCollisionEnemy == 0) InitializeLayerMask();
                return _noCollisionEnemy;
            }
        }

        public static LayerMask PlatformEnemy
        {
            get
            {
                if (_platformEnemy == 0) InitializeLayerMask();
                return _platformEnemy;
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

        public static LayerMask GrabbablePointLayerMask
        {
            get
            {
                if (_grabbablePointLayerMask == 0) InitializeLayerMask();
                return _grabbablePointLayerMask;
            }
        }

        [RuntimeInitializeOnLoadMethod]
        private static void InitializeLayerMask()
        {
            _playerLayerMask = LayerMask.GetMask("Player");
            _playerDodgeLayerMask = LayerMask.GetMask("PlayerDodge");
            _groundLayerMask = LayerMask.GetMask("Ground");
            _enemyLayerMask = LayerMask.GetMask("Enemy");
            _noCollisionEnemy = LayerMask.GetMask("NoCollisionEnemy");
            _platformEnemy = LayerMask.GetMask("PlatformEnemy");
            _passThroughPlatformLayerMask = LayerMask.GetMask("PassThroughPlatform");
            _grabbablePointLayerMask = LayerMask.GetMask("GrabbablePoint");
        }
    }
}