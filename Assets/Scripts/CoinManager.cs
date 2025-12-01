using UnityEngine;
using TMPro;
using System.Text;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public TMP_Text coinText;

    [SerializeField]
    private long currentCoin = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else if (Instance != this)
        {
            Destroy(transform.root.gameObject);
        }
    }

    void Start()
    {
        if (currentCoin == 0)
        {
            currentCoin = 2145678900;
        }

        UpdateCoinDisplay();

        if (coinText != null)
        {
            Debug.Log($"[CoinManager] 초기 자산 설정 완료: {coinText.text}");
        }
    }

    public void AddCoin(int amount)
    {
        currentCoin += amount;
        UpdateCoinDisplay();
    }

    public long GetCurrentCoin()
    {
        return currentCoin;
    }

    private void UpdateCoinDisplay()
    {
        if (coinText == null)
        {
            Debug.LogError("[CoinManager] UI 텍스트가 연결되지 않았습니다!");
            return;
        }

        string formattedMoney = FormatMoneyKorean(currentCoin);
        coinText.text = formattedMoney;
    }

    private string FormatLessThanTenThousand(int amount)
    {
        if (amount == 0) return "";

        int thousands = amount / 1000;
        int hundreds = (amount % 1000) / 100;
        int tens = (amount % 100) / 10;
        int ones = amount % 10;

        StringBuilder sb = new StringBuilder();

        if (thousands > 0) sb.Append(thousands).Append("천 ");
        if (hundreds > 0) sb.Append(hundreds).Append("백 ");
        if (tens > 0) sb.Append(tens).Append("십 ");
        if (ones > 0) sb.Append(ones);

        return sb.ToString().Trim();
    }

    private string FormatMoneyKorean(long money)
    {
        if (money < 0) return "0원";

        string[] units = { "", "만", "억" };
        long[] unitValues = { 1, 10000, 100000000 };

        StringBuilder sb = new StringBuilder();
        long tempMoney = money;

        for (int i = units.Length - 1; i >= 1; i--)
        {
            long unitValue = unitValues[i];
            long unitAmount = tempMoney / unitValue;

            if (unitAmount > 0)
            {
                sb.Append(unitAmount.ToString());
                sb.Append(units[i]);
                sb.Append(" ");
                tempMoney %= unitValue;
            }
        }

        if (tempMoney > 0)
        {
            string remainderKorean = FormatLessThanTenThousand((int)tempMoney);
            sb.Append(remainderKorean);
        }

        if (sb.Length == 0) return "0원";

        return sb.ToString().Trim() + "원";
    }
    
        public bool TrySpend(long amount)
    {
        if (amount <= 0) return false;
        if (currentCoin < amount) return false;
        currentCoin -= amount;
        UpdateCoinDisplay();
        return true;
    }

    public void AddCoin(long amount)
    {
        if (amount <= 0) return;
        currentCoin += amount;
        UpdateCoinDisplay();
    }


}
