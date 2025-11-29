using UnityEngine;
using TMPro;

public class StockTradePanel : MonoBehaviour
{
    [Header("Selection Source")]
    public StockDetailPanel detailPanel; // FrontLeft 선택 상태(현재 인덱스)

    [Header("UI")]
    public TMP_Text selectedNameText;
    public TMP_Text currentPriceText;
    public TMP_Text qtyText;
    public TMP_Text totalText;
    public TMP_Text feedbackText;

    [Header("Config")]
    public int maxQty = 9999;

    private string qtyStr = "";

    private void OnEnable()
    {
        RefreshAll();
    }

    private void Update()
    {
        // 가격은 30초마다 바뀌니 프리뷰만 자주 갱신(가벼움)
        RefreshAll();
    }

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

    private long GetCurrentPrice(StockId id)
    {
        if (StockMarketManager.Instance == null) return 0;
        // float→long 반올림
        return (long)System.Math.Round(StockMarketManager.Instance.GetPrice(id));
    }

    private int GetQty()
    {
        if (string.IsNullOrEmpty(qtyStr)) return 0;
        if (int.TryParse(qtyStr, out int v)) return v;
        return 0;
    }

    private void RefreshAll()
    {
        var sid = GetSelectedStockId();
        if (sid == null)
        {
            if (selectedNameText) selectedNameText.text = "선택된 종목 없음";
            if (currentPriceText) currentPriceText.text = "-";
            if (totalText) totalText.text = "-";
            if (qtyText) qtyText.text = string.IsNullOrEmpty(qtyStr) ? "0" : qtyStr;
            return;
        }

        long price = GetCurrentPrice(sid.Value);

        if (selectedNameText) selectedNameText.text = sid.Value.ToString();
        if (currentPriceText) currentPriceText.text = price.ToString("#,0");

        int qty = GetQty();
        long total = price * (long)qty;

        if (qtyText) qtyText.text = qty.ToString();
        if (totalText) totalText.text = total.ToString("#,0");
    }

    // === Keypad handlers ===
    public void PressDigit(string digit)
    {
        if (digit.Length != 1 || digit[0] < '0' || digit[0] > '9') return;

        if (qtyStr.Length == 0 && digit == "0") return; // 선행 0 방지

        if (qtyStr.Length >= 4) return; // maxQty=9999 기준 (원하면 늘려도 됨)
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
    public void Buy()
    {
        feedbackText.text = "";
        var sid = GetSelectedStockId();
        if (sid == null) { feedbackText.text = "종목을 먼저 선택하세요"; return; }

        int qty = GetQty();
        if (qty <= 0) { feedbackText.text = "수량을 입력하세요"; return; }

        long price = GetCurrentPrice(sid.Value);

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
    }

    public void Sell()
    {
        feedbackText.text = "";
        var sid = GetSelectedStockId();
        if (sid == null) { feedbackText.text = "종목을 먼저 선택하세요"; return; }

        int qty = GetQty();
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
    }
}
