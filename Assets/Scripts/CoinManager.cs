using System;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    // ✅ 코인 변경 이벤트
    public event Action<long> OnCoinChanged;

    public TMP_Text coinText;

    [SerializeField] private float timeUntilSceneChange = 5f;
    private float timeInNegative;
    private bool isNegative = false;

    [SerializeField] private long currentCoin = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(transform.root.gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "RiverScene")
        {
            transform.root.gameObject.SetActive(false);
            Debug.Log("[CoinManager] RiverScene 로드 완료, Global UI 숨김.");
        }
        else
        {
            transform.root.gameObject.SetActive(true);
            Debug.Log("[CoinManager] 새로운 씬 로드 완료, Global UI 활성화.");
        }

        // ✅ 씬 바뀌어도 구독자/UI가 다시 그리도록 강제 통지
        RefreshCoinUIAndNotify();
    }

    void Start()
    {
        RefreshCoinUIAndNotify();

        if (coinText != null)
            Debug.Log($"[CoinManager] 초기 자산 설정 완료: {FormatMoneyKorean(currentCoin)}");
    }

    void Update()
    {
        if (isNegative)
        {
            timeInNegative += Time.deltaTime;
            if (timeInNegative >= timeUntilSceneChange)
            {
                Debug.Log("[CoinManager] 코인 부족 시간 초과 (5초), RiverScene으로 이동합니다.");
                LoadRiverScene();
            }
        }
    }

    private void LoadRiverScene()
    {
        SceneManager.LoadScene("RiverScene");
    }

    // ✅ 코인 변경 공통 처리(유일한 진실)
    private void ApplyCoinChanged()
    {
        UpdateCoinDisplay();   // (CoinManager가 들고 있는 텍스트)
        CheckCoinStatus();     // 음수 타이머 상태
        OnCoinChanged?.Invoke(currentCoin);  // (다른 UI들)
    }

    // ✅ 씬 로드 등 "값은 그대로지만 UI를 다시" 상황
    public void RefreshCoinUIAndNotify()
    {
        UpdateCoinDisplay();
        OnCoinChanged?.Invoke(currentCoin);
    }

    // 돈이 부족하면 실패
    public bool TrySpend(long amount)
    {
        if (amount <= 0) return false;

        if (currentCoin < amount)
        {
            Debug.Log("[CoinManager] 잔액 부족으로 구매 실패.");
            return false;
        }

        currentCoin -= amount;
        ApplyCoinChanged();
        return true;
    }

    // ✅ 여기서부터가 원래 버그 지점: 양수->양수 변화에서도 항상 갱신/이벤트 발생해야 함
    public void AddCoin(long amount)
    {
        currentCoin += amount;
        ApplyCoinChanged();
    }

    public void AddCoin(int amount) => AddCoin((long)amount);

    public long GetCurrentCoin() => currentCoin;

    private void CheckCoinStatus()
    {
        if (currentCoin < 0 && !isNegative)
        {
            isNegative = true;
            timeInNegative = 0f;
            Debug.Log("[CoinManager] 코인 마이너스 감지, 5초 후 씬 전환 타이머 시작.");
        }
        else if (currentCoin >= 0 && isNegative)
        {
            isNegative = false;
            timeInNegative = 0f;
            Debug.Log("[CoinManager] 코인 회복 감지, 씬 전환 타이머 중지.");
        }
    }

    private void UpdateCoinDisplay()
    {
        if (coinText == null) return; // 씬에 따라 없을 수 있으니 조용히 스킵
        coinText.text = FormatMoneyKorean(currentCoin);
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

    // ✅ 다른 UI도 쓰기 좋게 public으로 열어둠
    public string FormatMoneyKorean(long money)
    {
        if (money == 0) return "0원";

        bool neg = money < 0;
        long absMoney = System.Math.Abs(money);

        string[] units = { "", "만", "억" };
        long[] unitValues = { 1, 10000, 100000000 };

        StringBuilder sb = new StringBuilder();
        long tempMoney = absMoney;

        for (int i = units.Length - 1; i >= 1; i--)
        {
            long unitValue = unitValues[i];
            long unitAmount = tempMoney / unitValue;

            if (unitAmount > 0)
            {
                sb.Append(unitAmount.ToString()).Append(units[i]).Append(" ");
                tempMoney %= unitValue;
            }
        }

        if (tempMoney > 0)
            sb.Append(FormatLessThanTenThousand((int)tempMoney));

        string result = sb.ToString().Trim() + "원";
        return neg ? "-" + result : result;
    }
}
