using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー用ジャンプコンポーネント
/// </summary>
[RequireComponent(typeof(PIsGround))]
[RequireComponent(typeof(Rigidbody))]
public class PJump : MonoBehaviour
{
    #region Properties
    #endregion

    #region Inspector Variables
    [Header("ジャンプ関連の値")]
    [Tooltip("ジャンプ力"), SerializeField]
    private float _jumpPower = 2f;
    [InputName, SerializeField]
    private string _jumpButton = "";
    #endregion

    #region Component Member Variables
    private PIsGround _pIsGround = default;
    private Rigidbody _rigidbody = default;
    #endregion

    #region Member Variables
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
        Jump();
    }
    #endregion

    #region Enums
    #endregion

    #region Private Methods
    private void Init()
    {
        _pIsGround = GetComponent<PIsGround>();
        _rigidbody = GetComponent<Rigidbody>();
    }
    private void Jump()
    {
        // 接地していて、かつ、Jumpするためのボタンが押されたときに実行する
        if (_pIsGround.IsGround && Input.GetButtonDown(_jumpButton))
        {
            Debug.Log("OnJump!");

            _rigidbody.velocity = Vector3.zero;
            _rigidbody.velocity = Vector3.up * _jumpPower;
        }
    }
    #endregion
}