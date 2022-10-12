using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーの移動を制御するコンポーネント version.2
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PIsGround))]
public class PMove2 : MonoBehaviour
{
    #region Properties
    #endregion

    #region Inspector Variables
    [Header("移動を制御するにあたって必要な値")]
    [Range(0f, 40f), Tooltip("このキャラクターに加える重力"), SerializeField]
    float _gravity;
    [Tooltip("X軸移動を制御するボタンの名前"), InputName, SerializeField]
    private string _moveButtonH = "";
    [Tooltip("Z軸移動を制御するボタンの名前"), InputName, SerializeField]
    private string _moveButtonV = "";
    [Tooltip("ダッシュ時の加速度"), SerializeField]
    private float _dashAcceleration = 1.5f;
    [Tooltip("ダッシュを制御するボタンの名前"), InputName, SerializeField]
    private string _dashButtonName = "";
    #endregion

    #region Member Variables
    //===== Components =====//
    private Rigidbody _rigidbody = default;
    private PIsGround _pIsGround = default;
    //===== Other =====//
    private Quaternion _targetRotation = default;
    private float _rotationSpeed = 600f;
    #endregion

    #region Constant
    /// <summary> 通常の移動スピード </summary>
    private const float _nomalSpeed = 1.0f;
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
        _pIsGround = GetComponent<PIsGround>();
    }
    private void Move()
    {
        float speed = 0f;
        Vector3 newVelocity =
            new Vector3(Input.GetAxisRaw(_moveButtonH), 0, Input.GetAxisRaw(_moveButtonV)).normalized;

        //　メインカメラを基準に方向を決める。
        newVelocity = Camera.main.transform.TransformDirection(newVelocity);
        // 移動方向を向く
        if (newVelocity.magnitude > 0.5f)
        {
            _targetRotation = Quaternion.LookRotation(newVelocity, Vector3.up);
            speed = Input.GetButton(_dashButtonName) ? _dashAcceleration : _nomalSpeed;
        }
        // 時間を掛けて向きを変更する。(一フレームで向きを変えるのはおかしいので)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

        // 上の処理でローテーションがおかしな事になっているので調整する。
        var rotation = transform.rotation;
        rotation.x = 0f;
        rotation.z = 0f;
        transform.rotation = rotation;

        // 坂道移動を補正する。
        // 地面の情報を取得する
        var groundInfo = _pIsGround.GetIsGround();
        // 接地箇所
        var groundedPoint = groundInfo.point;
        // 接地箇所の法線ベクトル
        var grondNomal = groundInfo.normal;
        Debug.Log(grondNomal);

        // 接地箇所の法線ベクトルを進行方向に合わせる
        newVelocity.y = 0f;
        newVelocity = Vector3.Cross(grondNomal, Vector3.Cross(newVelocity, grondNomal));

        //速度を与える
        //newVelocity.y = 0f;
        //_rigidbody.velocity =
        //    newVelocity * speed * PStatusManager.Instance.TotalStatus._moveSpeed +
        //    Vector3.up * _rigidbody.velocity.y;

        if (_pIsGround.IsGround && newVelocity.magnitude > 0.5f)
        {
            if (_rigidbody.velocity.y > 0f)
            {
                _rigidbody.velocity =
                    (Vector3.right * newVelocity.x + Vector3.forward * newVelocity.z) *
                    speed * PStatusManager.Instance.TotalStatus._moveSpeed +
                    Vector3.up * newVelocity.y;
            }
            else
            {
                _rigidbody.velocity =
                    (Vector3.right * newVelocity.x + Vector3.forward * newVelocity.z) *
                    speed * PStatusManager.Instance.TotalStatus._moveSpeed +
                    Vector3.up * newVelocity.y + Vector3.up * _rigidbody.velocity.y;
            }
        }
        else
        {
            _rigidbody.velocity = Vector3.up * _rigidbody.velocity.y;
        }

    }
    #endregion
}