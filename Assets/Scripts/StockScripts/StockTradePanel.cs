//(키패드 입력 + 매수/매도 버튼)
using UnityEngine;
using TMPro;

public class StockTradePanel : MonoBehaviour
{
    [Header("Selection Source")]
    public StockDetailPanel detailPanel; // FrontLeft에서 선택된 인덱스

    [Header("UI")]
    public TMP_Text selectedNameText;
    public TMP_Text currentPriceText;
    public TMP_Text qtyText;
    public TMP_Text totalText;
    public TMP_Text feedbackText;

    [Header("Config")]
    public int maxQty = 9999;

    private string qtyStr = "";

    private void OnEnable() => RefreshAll();

    private StockId? GetSelectedStockId()
    {
        if (detailPanel == null) return null;
        int idx = detailPanel.CurrentIndex; // 0:A 1:B 2:C 3:D 4:T
        if (idx < 0) return null;

        return idx switch
        {
            0 => StockId.A,
            1 => StockId.B,
            2 => StockId.C,
            3 => StockId.D,
            4 => StockId.T,
            _ => null
        };
    }
        private void Start()
    {
        Debug.Log("[Trade] StockTradePanel Start");
    }


    private long GetCurrentPrice(StockId id)
    {
        if (StockMarketManager.Instance == null) return 0;
        return (long)System.Math.Round(StockMarketManager.Instance.GetPrice(id));
    }

    private int GetQty()
    {
        if (string.IsNullOrEmpty(qtyStr)) return 0;
        return int.TryParse(qtyStr, out int v) ? v : 0;
    }

    private void RefreshAll()
    {
        var sid = GetSelectedStockId();
        int qty = GetQty();

        if (qtyText) qtyText.text = qty.ToString();

        if (sid == null)
        {
            if (selectedNameText) selectedNameText.text = "선택된 종목 없음";
            if (currentPriceText) currentPriceText.text = "-";
            if (totalText) totalText.text = "-";
            return;
        }

        long price = GetCurrentPrice(sid.Value);
        long total = price * (long)qty;

        if (selectedNameText) selectedNameText.text = sid.Value.ToString();
        if (currentPriceText) currentPriceText.text = price.ToString("#,0");
        if (totalText) totalText.text = total.ToString("#,0");
    }

    // === Keypad handlers ===
    public void PressDigit(string digit)
    {
        if (digit.Length != 1 || digit[0] < '0' || digit[0] > '9') return;
        if (qtyStr.Length == 0 && digit == "0") return; // 선행 0 방지
        if (qtyStr.Length >= 4) return;

        qtyStr += digit;

        int qty = GetQty();
        if (qty > maxQty) qtyStr = maxQty.ToString();

        RefreshAll();
    }

    public void Backspace()
    {
        if (qtyStr.Length > 0) qtyStr = qtyStr.Substring(0, qtyStr.Length - 1);
        RefreshAll();
    }

    public void ClearQty()
    {
        qtyStr = "";
        RefreshAll();
    }

    // === Trade ===
    // 참고: 빨강=Buy, 초록=Sell로 이벤트만 연결해주면 됨
    public void Buy()

    {
        Debug.Log("[Trade] Buy pressed");

        if (feedbackText == null) { Debug.LogError("[Trade] feedbackText not assigned"); return; }

        feedbackText.text = "";

        var sid = GetSelectedStockId();
        int qty = GetQty();
        Debug.Log($"[Trade] sid={(sid.HasValue ? sid.Value.ToString() : "null")} qty={qty}");

        if (sid == null) { feedbackText.text = "종목을 먼저 선택하세요"; return; }
        if (qty <= 0) { feedbackText.text = "수량을 입력하세요"; return; }

        long price = GetCurrentPrice(sid.Value);
        long cost = price * (long)qty;

        var cm = CoinManager.Instance;
        Debug.Log($"[Trade] price={price} cost={cost} coin={(cm != null ? cm.GetCurrentCoin() : -1)}");

        if (StockPortfolioManager.Instance == null) { feedbackText.text = "PortfolioManager 없음"; return; }

        if (StockPortfolioManager.Instance.Buy(sid.Value, qty, price, out string msg))
        {
            feedbackText.text = msg;
            ClearQty();
        }
        else
        {
            feedbackText.text = msg;
        }

        RefreshAll();
    }

    public void Sell()
    {
        if (feedbackText == null) { Debug.LogError("[Trade] feedbackText not assigned"); return; }

        feedbackText.text = "";
        Debug.Log("[Trade] Sell pressed");

        var sid = GetSelectedStockId();
        int qty = GetQty();

        if (sid == null) { feedbackText.text = "종목을 먼저 선택하세요"; return; }
        if (qty <= 0) { feedbackText.text = "수량을 입력하세요"; return; }

        long price = GetCurrentPrice(sid.Value);

        if (StockPortfolioManager.Instance == null) { feedbackText.text = "PortfolioManager 없음"; return; }

        if (StockPortfolioManager.Instance.Sell(sid.Value, qty, price, out string msg))
        {
            feedbackText.text = msg;
            ClearQty();
        }
        else
        {
            feedbackText.text = msg;
        }

        RefreshAll();
    }
}
