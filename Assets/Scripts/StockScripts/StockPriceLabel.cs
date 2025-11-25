using UnityEngine;
using UnityEngine.UI; // Text
// TextMeshPro 쓰면 using TMPro; TMP_Text 로 타입만 바꾸면 됨

public class StockPriceLabel : MonoBehaviour
{
    public StockId stockId;
    public Text priceText;

    private void Start()
    {
        // 처음에 한 번 표시
        Refresh();

        // 주가 업데이트될 때마다 다시 표시
        if (StockMarketManager.Instance != null)
        {
            StockMarketManager.Instance.OnPricesUpdated += Refresh;
        }
    }

    private void OnDestroy()
    {
        if (StockMarketManager.Instance != null)
        {
            StockMarketManager.Instance.OnPricesUpdated -= Refresh;
        }
    }

    private void Refresh()
    {
        if (priceText == null || StockMarketManager.Instance == null) return;
        float price = StockMarketManager.Instance.GetPrice(stockId);
        priceText.text = $"{price:0}";
    }
}
