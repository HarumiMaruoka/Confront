using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackArrowController : MonoBehaviour
{
    [Tooltip("テスト用ダメージ値"), SerializeField]
    private float _testDamageValue;

    private Rigidbody _rigidbody = null;
    /// <summary> 矢の発射角度 </summary>
    private Vector3 _shootAngle = default;
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
    public void Setup(GameObject nomalArrow, Collider[] nonContactTarget, float shootPower, Vector3 shootAngle)
    {
        _nomalArrow = nomalArrow;
        _nonContactTarget = nonContactTarget;
        _shootPower = shootPower;
        _shootAngle = shootAngle;
        // 角度を補正する
        transform.rotation = Quaternion.FromToRotation(Vector3.down, shootAngle);
    }

    /// <summary> 生成時処理 </summary>
    private void Start()
    {
        // 初速度を与える
        GetComponent<Rigidbody>().AddForce(_shootAngle.normalized * _shootPower, ForceMode.Impulse);
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
        // 非攻撃用の"矢"（見た目だけの矢）を、接触相手の子オブジェクトとしてインスタンシエイトする。
        var arrow = Instantiate(_nomalArrow, transform.position, transform.rotation);
        // サイズを調整する。
        arrow.transform.SetParent(other.transform, true);
        Destroy(arrow, 60f); // 一分後に非攻撃用の"矢"（見た目だけの矢）を破棄する。
        // 自身を破棄する。
        Destroy(this.gameObject);
    }
}