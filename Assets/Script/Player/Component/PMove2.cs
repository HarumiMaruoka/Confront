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
    [Header("移動制御")]
    [Tooltip("X軸移動を制御するボタンの名前"), InputName, SerializeField]
    private string _moveButtonH = "";
    [Tooltip("Z軸移動を制御するボタンの名前"), InputName, SerializeField]
    private string _moveButtonV = "";
    [Tooltip("ダッシュ時の加速度"), SerializeField]
    private float _dashAcceleration = 1.5f;
    [Tooltip("ダッシュを制御するボタンの名前"), InputName, SerializeField]
    private string _dashButtonName = "";

    [Header("ジャンプ制御")]
    [Tooltip("ジャンプ力"), SerializeField]
    private float _jumpPower = 2f;
    [InputName, SerializeField]
    private string _jumpButton = "";

    [Header("姿勢制御")]
    [Tooltip("デバッグ表示するかどうか"), SerializeField]
    private bool _isGizmo = false;
    [Tooltip("デバッグ表示するレイの色"), SerializeField]
    private Color _rayColor = default;
    [Tooltip("プレイヤーからまっすぐ下に伸ばすレイの最大距離"), SerializeField]
    private float _groundCheckMaxDistance = 1f;
    [Tooltip("接地対象"), SerializeField]
    private LayerMask _groundLayer = default;
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
    private void OnDrawGizmos()
    {
        if (_isGizmo)
        {
            // Debug.DrawRay(transform.position, Vector3.down, _rayColor, _groundCheckMaxDistance);
            Debug.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckMaxDistance, _rayColor);
        }
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
        MoveGround();
        Jump();
    }

    private void MoveGround()
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
        // 接地箇所の法線ベクトルを取得（自分からまっすぐ下にRayを飛ばす）
        Ray ray = new Ray(transform.position, Vector3.down);
        var groundNomal = Physics.Raycast(ray, out RaycastHit hit, _groundCheckMaxDistance, _groundLayer);
        // 接地箇所の法線ベクトルを進行方向に合わせる
        // （やってることは坂を表現できる面とペンを使用して想像してみるとわかりやすい）
        newVelocity.y = 0f;
        var groundVelocity = Vector3.Cross(hit.normal, Vector3.Cross(newVelocity, hit.normal)).normalized;

        //速度を与える

        if (_pIsGround.IsGround && newVelocity.magnitude > 0.5f)
        {
            if (_rigidbody.velocity.y > 0f)
            {
                _rigidbody.velocity =
                    (Vector3.right * newVelocity.x + Vector3.forward * newVelocity.z) *
                    speed * PStatusManager.Instance.TotalStatus._moveSpeed +
                    Vector3.up * groundVelocity.y;
            }
            else
            {
                _rigidbody.velocity =
                    (Vector3.right * newVelocity.x + Vector3.forward * newVelocity.z) *
                    speed * PStatusManager.Instance.TotalStatus._moveSpeed +
                    Vector3.up * groundVelocity.y + Vector3.up * _rigidbody.velocity.y;
            }
        }
        else
        {
            _rigidbody.velocity =
                newVelocity * speed * PStatusManager.Instance.TotalStatus._moveSpeed +
                Vector3.up * _rigidbody.velocity.y;
        }
    }

    private void Jump()
    {
        // 接地していて、かつ、Jumpするためのボタンが押されたときに実行する
        if (_pIsGround.IsGround && Input.GetButtonDown(_jumpButton))
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.velocity = Vector3.up * _jumpPower;
        }
    }
    #endregion
}