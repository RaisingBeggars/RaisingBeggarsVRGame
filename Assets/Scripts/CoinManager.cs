using UnityEngine;
using TMPro;
using System.Text;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    [Header("UI 연결")]
    public TMP_Text coinText; // 금액을 표시할 UI 텍스트 (VR 카메라 앞 UI)

    [Header("코인 데이터")]
    [SerializeField]
    private long currentCoin = 0; // 실제 자산 데이터 

    void Awake()
    {
        // 싱글톤 패턴: 게임 내에 CoinManager는 단 하나만 존재해야 함
        if (Instance == null)
        {
            Instance = this;
            // 중요: 씬이 바뀌어도(로비 -> 인게임 등) 자산 데이터가 사라지지 않게 함
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            // 만약 다른 씬에서 넘어왔는데 이미 CoinManager가 있다면, 중복된 새 것은 파괴
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 테스트용: 저장된 돈이 없으면 초기 자금 설정
        if (currentCoin == 0)
        {
            currentCoin = 2145678900; // 약 21억 (테스트 값)
        }

        // 게임 시작 시 UI 갱신
        UpdateCoinDisplay();

        if (coinText != null)
        {
            Debug.Log($"[CoinManager] 초기 자산 설정 완료: {coinText.text}");
        }
    }

    //  주식 매수/매도 시 이 함수를 호출 ★★★
    // amount가 양수면 돈 증가(매도), 음수면 돈 감소(매수)
    // 예: CoinManager.Instance.AddCoin(-10000); // 1만원 차감
    public void AddCoin(int amount)
    {
        currentCoin += amount;
        UpdateCoinDisplay(); // 데이터 변경 후 즉시 UI 반영
    }

    // 현재 잔액 확인 ★★★
    // 예: if(CoinManager.Instance.GetCurrentCoin() >= 주식가격) { 매수실행 }
    public long GetCurrentCoin()
    {
        return currentCoin;
    }

    // 내부적으로 사용하는 UI 갱신 함수
    private void UpdateCoinDisplay()
    {
        if (coinText == null)
        {
            Debug.LogError("[CoinManager] UI 텍스트가 연결되지 않았습니다!");
            return;
        }

        // 한국 화폐 단위(억, 만, 원)로 포맷팅하여 텍스트 적용
        string formattedMoney = FormatMoneyKorean(currentCoin);
        coinText.text = formattedMoney;
    }

    // --- 아래는 단순히 숫자를 '00억 00만 00원' 텍스트로 바꾸는 로직 (건드릴 필요 X) ---

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
}