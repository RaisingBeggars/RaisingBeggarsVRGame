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

        // 히스토리가 없으면 현재가 한 줄이라도 표시
        if (hist == null || hist.Count == 0)
        {
            float priceNow = StockMarketManager.Instance.GetPrice(stockId);
            historyText.text = priceNow > 0 ? FormatPrice(priceNow) : "-";
            return;
        }

        var sb = new StringBuilder();

        // ★ 최신 값이 "맨 아래"로 가도록, 오래된 값부터 순서대로 출력
        int linesToShow = Mathf.Min(hist.Count, maxLines);
        int startIdx = hist.Count - linesToShow;   // 보여줄 구간의 시작 인덱스

        for (int i = startIdx; i < hist.Count; i++)
        {
            float price = hist[i];
            string plainLine;
            string colorStart;
            const string colorEnd = "</color>";

            if (i == 0)
            {
                // 히스토리의 첫 값: 기준가, 변화량 없이 숫자만
                plainLine = FormatPrice(price);
                colorStart = "<color=#FFFFFF>";   // 흰색(원하면 다른 색으로)
            }
            else
            {
                float prev = hist[i - 1];
                float diff = price - prev;
                float diffPercent = prev != 0f ? diff / prev * 100f : 0f;

                string arrow;
                if (diff > 0f)
                {
                    arrow = "🔺";
                    colorStart = "<color=#C73838>";   // 빨간색 (상승)
                }
                else if (diff < 0f)
                {
                    arrow = "🔻";
                    colorStart = "<color=#3B3BB3>";   // 파란색 (하락)
                }
                else
                {
                    arrow = "⏺";
                    colorStart = "<color=#DDDDDD>";   // 보합 = 회색
                }

                string percentText = diffPercent.ToString("0.#");
                if (diffPercent > 0f) percentText = "+" + percentText;

                plainLine = $"{FormatPrice(price)} {arrow}({percentText}%)";
            }

            // ★ 한 줄 전체를 색 입혀서 누적
            sb.AppendLine(colorStart + plainLine + colorEnd);
        }

        // 마지막에 한 번만 Text에 넣기
        historyText.text = sb.ToString();
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
