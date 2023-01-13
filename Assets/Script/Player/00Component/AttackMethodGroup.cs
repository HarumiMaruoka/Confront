using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// 攻撃処理を記述するためのクラス。<br/>
    /// 攻撃処理をアニメーションからアニメーションイベントから
    /// 呼び出す為コンポーネントとして用意。<br/>
    /// public void Attack-IDNumber-ComboNumber(必要であれば,アニメーションイベントで使用できる型の引数) { 実装 }<br/>
    /// メソッド名は↑こんな感じで記述していく。（選択する際見やすくするため）<br/>
    /// 各攻撃,各コンボそれぞれに用意する？←何とか引数でコントロールできないか?
    /// </summary>
    public class AttackMethodGroup : MonoBehaviour
    {
        [SerializeField]
        private Helper.Raycast Attack00Ray = default;
        public void Attack00()
        {

        }
    }
}