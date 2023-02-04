using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrowController : MonoBehaviour
{
    [Tooltip("テスト用ダメージ値"), SerializeField]
    private float _testDamageValue;
    [Tooltip("矢の進行方向"), SerializeField]
    private Vector3 _shootAngle = default;

    private Rigidbody _rigidbody = null;
    /// <summary> 矢の初速度 </summary>
    private float _shootPower = default;
    /// <summary> 攻撃用ではない矢オブジェクト </summary>
    private GameObject _nomalArrow = default;
    /// <summary> 非接触対象 </summary>
    private Collider[] _nonContactTarget = default;

    /// <summary> 初期化処理 </summary>
    /// <param name="nomalArrow"> 攻撃用ではない矢オブジェクト </param>
    /// <param name="nonContactTarget"> 非接触対象 </param>
    /// <param name="shootPower"> 矢の初速度 </param>
    public void Setup(GameObject nomalArrow, Collider[] nonContactTarget, float shootPower)
    {
        _nomalArrow = nomalArrow;
        _nonContactTarget = nonContactTarget;
        _shootPower = shootPower;
    }

    /// <summary> 生成時処理 </summary>
    private void Start()
    {
        // 初速度を与える
        _rigidbody = gameObject.AddComponent<Rigidbody>();
        _rigidbody.AddForce(_shootAngle.normalized * _shootPower, ForceMode.Impulse);
    }

    /// <summary> 接触時処理 </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // 非接触対象の場合は無視する
        foreach (var e in _nonContactTarget)
        {
            if (e == other)
                return;
        }
        // 接触相手がITakenDamageを継承している場合, TakeDamage()を呼び出す。
        if (other.TryGetComponent(out ITakenDamage target))
        {
            target.TakenDamage(_testDamageValue);
        }
        // 非接触用の"矢"を、接触相手の子オブジェクトとしてインスタンシエイトする。
        Instantiate(_nomalArrow, transform.position, transform.rotation, other.transform);
        // 自身を破棄する。
        Destroy(this.gameObject);
    }
}