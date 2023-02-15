using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WeaponAnimController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController _playerController = default;

        private Animator _animator = null;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void OnAnimEnd(AnimType type)
        {
            _playerController.OnAnimEnd(type);
        }
        public void OnDisableWeapon()
        {
            if (_animator != null)
            {
                if (!_animator.IsInTransition(0))
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}