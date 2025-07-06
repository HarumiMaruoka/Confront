using System;
using UnityEngine;

public class CurrencyManager
{
    public static CurrencyManager Instance { get; private set; } = new CurrencyManager();

    public int Balance { get; private set; }

    public event Action<int> BalanceChanged;

    /// <summary>獲得（加算）。負の値は禁止。</summary>
    public void Earn(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        Balance += amount;
        BalanceChanged?.Invoke(Balance);
    }

    /// <summary>
    /// 支払い。残高が足りない場合は何もしないで false。
    /// 成功時は残高を減らして true。
    /// </summary>
    public bool TrySpend(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (Balance < amount) return false;

        Balance -= amount;
        BalanceChanged?.Invoke(Balance);
        return true;
    }

    /// <summary>支払い（減算）。残高が足りない場合は例外。</summary>
    public void Debit(int amount)
    {
        if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));
        if (Balance < amount) throw new InvalidOperationException("Insufficient balance for debit.");
        Balance -= amount;
        BalanceChanged?.Invoke(Balance);
    }

    /// <summary>デバッグや運営用に直接セット。</summary>
    public void Set(int amount)
    {
        Balance = Mathf.Max(0, amount);
        BalanceChanged?.Invoke(Balance);
    }

    private void Load()
    {
        throw new NotImplementedException("Load method is not implemented yet.");
    }

    private void Save()
    {
        throw new NotImplementedException("Save method is not implemented yet.");
    }
}
