using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動を制御するコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PMove1 : MonoBehaviour
{
    #region Properties
    #endregion

    #region Inspector Variables
    [Header("移動を制御するにあたって必要な値")]
    [Tooltip("ダッシュ時の加速度"), SerializeField]
    float _dashAcceleration = 1.5f;
    #endregion

    #region Member Variables
    private Rigidbody _rigidbody = default;
    private Quaternion _targetRotation = default;
    private float _rotationSpeed = 600f;
    #endregion

    #region Constant
    #endregion

    #region Events
    #endregion

    #region Unity Methods

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Move();
    }

    #endregion

    #region Enums
    #endregion

    #region Public Methods
    #endregion

    #region Private Methods
    private void Init()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Move()
    {
        float speed = 0f;
        Vector3 newVelocity =
            new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        //　メインカメラを基準に方向を決める。
        newVelocity = Camera.main.transform.TransformDirection(newVelocity);
        // 移動方向を向く
        if (newVelocity.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(newVelocity, Vector3.up);
            speed = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;
        }
        // 時間を掛けて向きを変更する。(一フレームで向きを変えるのはおかしいので)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

        var rotation = transform.rotation;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;


        //速度を与える
        newVelocity.y = 0;
        _rigidbody.velocity =
            newVelocity * speed * PStatusManager.Instance.TotalStatus._moveSpeed +
            Vector3.up * _rigidbody.velocity.y;
    }
    #endregion
}