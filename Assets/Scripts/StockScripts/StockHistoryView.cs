using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class StockHistoryView : MonoBehaviour
{
    public StockId stockId;       // 이 패널이 보여줄 회사
    public Text historyText;      // 여러 줄 기록
    public Text countdownText;    // "다음 변동까지 XXs"
    public int maxLines = 10;     // 화면에 최대 몇 줄까지 보여줄지

    private void Start()
    {
        Refresh();

        if (StockMarketManager.Instance != null)
            StockMarketManager.Instance.OnPricesUpdated += Refresh;
    }

    private void OnDestroy()
    {
        if (StockMarketManager.Instance != null)
            StockMarketManager.Instance.OnPricesUpdated -= Refresh;
    }

    private void Update()
    {
        UpdateCountdown();
    }

    private void Refresh()
    {
        if (historyText == null || StockMarketManager.Instance == null) return;

        IReadOnlyList<float> hist = StockMarketManager.Instance.GetHistory(stockId);

        // 히스토리가 아무 것도 없으면 현재가 한 줄이라도 보여줌
        if (hist == null || hist.Count == 0)
        {
            float priceNow = StockMarketManager.Instance.GetPrice(stockId);
            historyText.text = priceNow > 0 ? FormatPrice(priceNow) : "-";
            return;
        }

        var sb = new StringBuilder();

        // ★ 최신 값부터 거꾸로, 최대 maxLines 줄까지 출력
        int linesToShow = Mathf.Min(hist.Count, maxLines);
        for (int i = 0; i < linesToShow; i++)
        {
            int idx = hist.Count - 1 - i;   // 맨 뒤가 최신
            float price = hist[idx];
            string line;

            if (idx == 0)
            {
                // 맨 처음 값: 변화량 없이 숫자만
                line = FormatPrice(price);
            }
            else
            {
                float prev = hist[idx - 1];
                float diff = price - prev;
                float diffPercent = prev != 0f ? diff / prev * 100f : 0f;

                string arrow;
                if (diff > 0f) arrow = "🔺";
                else if (diff < 0f) arrow = "🔻";
                else arrow = "⏺";

                string percentText = diffPercent.ToString("0.#");
                if (diffPercent > 0f) percentText = "+" + percentText;

                line = $"{FormatPrice(price)} {arrow}({percentText}%)";
            }

            sb.AppendLine(line);
        }

        // ★ 루프 끝난 뒤에 딱 한 번만 텍스트에 넣기
        historyText.text = sb.ToString();

        // 디버그로 실제 들어간 문자열 확인해보고 싶으면 이 줄 잠깐 켜기
        // Debug.Log($"[{stockId}] history text:\n" + historyText.text);
    }

    private void UpdateCountdown()
    {
        if (countdownText == null || StockMarketManager.Instance == null) return;

        float remain = StockMarketManager.Instance.GetRemainingSeconds();
        countdownText.text = $"다음 변동까지 {remain:0}s";
    }

    private string FormatPrice(float value)
    {
        int intValue = Mathf.RoundToInt(value);
        return intValue.ToString("#,0");
    }
}
