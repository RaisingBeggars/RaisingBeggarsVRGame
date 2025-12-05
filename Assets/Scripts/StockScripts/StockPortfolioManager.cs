//(보유/평단/매수/매도)
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StockHolding
{
    public StockId id;
    public long shares;
    public long avgPrice;
}

public class StockPortfolioManager : MonoBehaviour
{
    public static StockPortfolioManager Instance { get; private set; }

    [SerializeField] private List<StockHolding> holdings = new();
    private readonly Dictionary<StockId, StockHolding> map = new();

    public event Action OnPortfolioChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        RebuildMap();
    }

    private void RebuildMap()
    {
        map.Clear();
        foreach (var h in holdings)
        {
            if (h == null) continue;
            map[h.id] = h;
        }
    }

    private StockHolding GetOrCreate(StockId id)
    {
        if (map.TryGetValue(id, out var h)) return h;

        h = new StockHolding { id = id, shares = 0, avgPrice = 0 };
        holdings.Add(h);
        map[id] = h;
        return h;
    }

    public StockHolding GetHolding(StockId id) => GetOrCreate(id);
    public long GetShares(StockId id) => GetOrCreate(id).shares;
    public long GetAvgPrice(StockId id) => GetOrCreate(id).avgPrice;

    public bool Buy(StockId id, int qty, long price, out string msg)
    {
        msg = "";
        if (qty <= 0) { msg = "수량이 0입니다."; return false; }
        if (price <= 0) { msg = "가격 오류"; return false; }
        if (CoinManager.Instance == null) { msg = "CoinManager 없음"; return false; }

        long cost = price * (long)qty;
        if (!CoinManager.Instance.TrySpend(cost))
        {
            msg = "잔액이 부족합니다.";
            return false;
        }

        var h = GetOrCreate(id);

        long prevShares = h.shares;
        long newShares = prevShares + qty;

        long prevCost = h.avgPrice * prevShares;
        long newCost = prevCost + cost;

        h.shares = newShares;
        h.avgPrice = (newShares > 0) ? (newCost / newShares) : 0;

        OnPortfolioChanged?.Invoke();
        msg = $"매수 완료: {qty}주";
        return true;
    }

    public bool Sell(StockId id, int qty, long price, out string msg)
    {
        msg = "";
        if (qty <= 0) { msg = "수량이 0입니다."; return false; }
        if (price <= 0) { msg = "가격 오류"; return false; }
        if (CoinManager.Instance == null) { msg = "CoinManager 없음"; return false; }

        var h = GetOrCreate(id);
        if (h.shares < qty)
        {
            msg = "보유 수량이 부족합니다.";
            return false;
        }

        long revenue = price * (long)qty;
        CoinManager.Instance.AddCoin(revenue);

        h.shares -= qty;
        if (h.shares == 0) h.avgPrice = 0;

        OnPortfolioChanged?.Invoke();
        msg = $"매도 완료: {qty}주";
        return true;
    }

    public IReadOnlyList<StockHolding> GetHoldingsAll() => holdings;

    // 보유하지 않은 주식들 처리
    public List<StockHolding> GetHoldingsOwned()
{
    var res = new List<StockHolding>();
    foreach (var h in holdings)
    {
        if (h == null) continue;
        if (h.shares > 0) res.Add(h);
    }
    return res;
}


    

}
