using System;
using UnityEngine;

[System.Serializable]
public abstract class StateMachineBase
{
    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update();
        }
    }
    [NonSerialized]
    protected IState _currentState;
    public IState CurrentState { get => _currentState; }

    public event Action<IState, IState> OnStateChanged = default;

    // 最初のステートを設定する。
    protected void Initialize(IState startState)
    {
        StateInit();

        _currentState = startState;
        startState.Enter();

        // ステート変化時に実行するアクション。
        // 引数に最初のステートを渡す。
        OnStateChanged?.Invoke(null, startState);

#if UNITY_EDITOR
        OnStateChanged +=
            (previousState, newState) =>
            {
                Debug.Log(
                    $"プレイヤーのステートが変更されました。\n" +
                    $"現在のステートは\"{newState.GetType().Name}\"です。");
            };
#endif
    }

    // ステートの遷移処理。引数に「次のステートの参照」を受け取る。
    public void TransitionTo(IState nextState)
    {
        if (nextState == null)
        {
            Debug.LogError($"引数に渡された {nextState} はnullです。遷移をキャンセルします。");
            return;
        }
        var previousState = _currentState;
        _currentState.Exit();      // 現在ステートの終了処理。
        _currentState = nextState; // 現在のステートの変更処理。
        nextState.Enter();         // 変更された「新しい現在ステート」のEnter処理。

        // ステート変更時のアクションを実行する。
        // 引数に「新しい現在ステート」を渡す。
        OnStateChanged?.Invoke(previousState, nextState);
    }
    protected abstract void StateInit();
}