//Row 프리팹에 붙일 스크립트
using TMPro;
using UnityEngine;

public class StockHoldingRowView : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text nameText;
    public TMP_Text sharesText;
    public TMP_Text avgPriceText;
    public TMP_Text currentPriceText;

    static string Won(long v) => $"{v:N0}원";

    public void Bind(string name, long shares, long avgPrice, long currentPrice)
    {
        if (nameText) nameText.text = name;
        if (sharesText) sharesText.text = shares.ToString("N0");
        if (avgPriceText) avgPriceText.text = Won(avgPrice);
        if (currentPriceText) currentPriceText.text = Won(currentPrice);
    }
}
