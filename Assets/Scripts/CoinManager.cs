using UnityEngine;
using TMPro;
using System.Text;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    [Header("UI 연결")]
    public TMP_Text coinText;
    [Header("코인 데이터")]
    [SerializeField]
    private long currentCoin = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            // 중복 인스턴스 처리
        }
    }

    void Start()
    {
        if (currentCoin == 0)
        {
            currentCoin = 2145678900; // 테스트 값
        }

        UpdateCoinDisplay();

        if (coinText != null)
        {
            Debug.Log($"[CoinManager] Start() 실행 완료. 초기 금액: {coinText.text}");
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
            Debug.LogError("[CoinManager] Coin Text (TMP_Text)가 연결되지 않았습니다. UI 업데이트 실패.");
            return;
        }

        string formattedMoney = FormatMoneyKorean(currentCoin);
        coinText.text = formattedMoney;
    }

    /// <summary>
    /// 10,000 미만의 숫자를 한국어 단위 (천, 백, 십, 일)로 포맷합니다.
    /// 예: 8900 -> "8천 9백", 5 -> "5"
    /// </summary>
    private string FormatLessThanTenThousand(int amount)
    {
        if (amount == 0) return "";

        // 천/백/십/일 단위 추출
        int thousands = amount / 1000;
        int hundreds = (amount % 1000) / 100;
        int tens = (amount % 100) / 10;
        int ones = amount % 10;

        StringBuilder sb = new StringBuilder();

        if (thousands > 0)
        {
            sb.Append(thousands).Append("천 ");
        }

        if (hundreds > 0)
        {
            sb.Append(hundreds).Append("백 ");
        }

        if (tens > 0)
        {
            sb.Append(tens).Append("십 ");
        }

        // 1의 자리 (1~9)가 남아있다면 숫자만 추가
        if (ones > 0)
        {
            sb.Append(ones);
        }

        // 10000 미만의 전체 금액이 0이 아닐 경우, 숫자만 남아있는 경우를 대비하여 공백 제거
        return sb.ToString().Trim();
    }


    /// <summary>
    /// 금액을 한국식 단위 (만, 억)로 포맷하고, 맨 뒤에 '원'을 추가합니다.
    /// </summary>
    private string FormatMoneyKorean(long money)
    {
        if (money < 0) return "0원"; // 0원이나 음수 처리

        // 한국 단위 (10^4 승수)
        string[] units = { "", "만", "억" };
        long[] unitValues = { 1, 10000, 100000000 };

        StringBuilder sb = new StringBuilder();
        long tempMoney = money;

        // 가장 큰 단위(억)부터 순서대로 처리 (i=2: 억, i=1: 만)
        for (int i = units.Length - 1; i >= 1; i--)
        {
            long unitValue = unitValues[i];
            // '만' 단위와 '억' 단위에 들어갈 금액 (예: 21억 4567만)
            long unitAmount = tempMoney / unitValue;

            if (unitAmount > 0)
            {
                // 숫자와 단위를 추가. 만/억 단위는 콤마 없이 깔끔하게 출력
                sb.Append(unitAmount.ToString());
                sb.Append(units[i]);
                sb.Append(" ");

                // 사용된 금액은 나머지 연산자로 제외
                tempMoney %= unitValue;
            }
        }

        // **NEW LOGIC START: 10,000 미만 금액 처리**
        // tempMoney는 이제 0~9999 사이의 값을 가집니다.
        if (tempMoney > 0)
        {
            string remainderKorean = FormatLessThanTenThousand((int)tempMoney);
            sb.Append(remainderKorean);
        }
        // **NEW LOGIC END**

        if (sb.Length == 0)
        {
            return "0원";
        }

        // 마지막 공백 제거 후 '원' 추가
        return sb.ToString().Trim() + "원";
    }
}