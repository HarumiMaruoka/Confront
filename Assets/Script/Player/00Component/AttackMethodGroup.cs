using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 攻撃処理を記述するためのクラス。<br/>
    /// 攻撃処理をアニメーションからアニメーションイベントから
    /// 呼び出す為コンポーネントとして用意。<br/>
    /// public void Attack-WeaponType-OrderNumber(必要であれば,アニメーションイベントで使用できる型の引数) { 実装 }<br/>
    /// メソッド名は↑こんな感じで記述していく。（選択する際見やすくするため）<br/>
    /// 各攻撃,各コンボそれぞれに用意する？←何とか引数でコントロールできないか?
    /// </summary>
    public class AttackMethodGroup : MonoBehaviour
    {
        // ========== 初期化処理 ========== //
        private void Start()
        {
            Setup();
        }
        /// <summary>
        /// 初期化処理
        /// </summary>
        private void Setup()
        {
            _gunRay.Init(transform);
            _nomalSword.Init(transform);
        }

        // =========== Gizmo関連 ========== //
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            _gunRay.OnDrawGizmo(transform);
            _nomalSword.OnDrawGizmos(transform);
        }
#endif

        // ============ 実装部 ============ //

        // ================ Gun ================ //
        [SerializeField]
        private Helper.Raycast _gunRay = default;
        [SerializeField, Range(1, 10)]
        private float _testGunDamage = 1;
        /// <summary>
        /// 前方にレイを飛ばしヒットした対象にダメージを加える
        /// </summary>
        public void AttackGun()
        {
            if (_gunRay.IsHit())
            {
                if (_gunRay.Result.collider.TryGetComponent(out ITakenDamage target))
                {
                    target.TakenDamage(_testGunDamage);
                }
            }
        }

        // ============= NomalSword ============= //
        [SerializeField]
        private Helper.OverLapBox _nomalSword = default;
        [SerializeField, Range(1, 100)]
        private float _testNomalSwordDamage = 1;
        /// <summary>
        /// 前方にレイを飛ばしヒットした対象にダメージを加える
        /// </summary>
        public void AttackNomalSword()
        {
            // 上半身と下半身で同じアニメーションを使用しているため、
            // アニメーションイベントが二回呼ばれ、二倍のダメージが敵に入る。
            // これは仕様です。
            // (二回処理が走っているので、もしパフォーマンスを向上させたいと感じたら。全身用のアニメーションで再生する。)
            if (_nomalSword.IsHit())
            {
                foreach (var e in _nomalSword.GetCollider())
                {
                    if (e.TryGetComponent(out ITakenDamage target))
                    {
                        target.TakenDamage(_testNomalSwordDamage);
                    }
                }
            }
        }
    }
}