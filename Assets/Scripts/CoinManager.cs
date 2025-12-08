using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;
using System.Collections; // 코루틴 사용을 위해 필수
using System; // Action 및 Math 사용

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    // ✅ 코인 변경 이벤트 (다른 스크립트에서 구독 가능)
    public event Action<long> OnCoinChanged;

    [Header("UI 연결")]
    public TMP_Text coinText;       // 현재 자산 텍스트
    public TMP_Text countdownText;  // ⭐ 카운트다운 텍스트 (새로 추가)

    [Header("게임 설정")]
    [SerializeField] private long currentCoin = 0;
    [SerializeField] private string endingSceneName = "RiverScene"; // 이동할 씬 이름

    private bool isCountingDown = false; // 카운트다운 중복 방지

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // UI를 포함한 루트 오브젝트 유지
            DontDestroyOnLoad(transform.root.gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            // 시작 시 카운트다운 텍스트는 숨김
            if (countdownText != null) countdownText.gameObject.SetActive(false);
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
        // 특정 씬(RiverScene 등)에서 Global UI 숨기기
        if (scene.name == "RiverScene" || scene.name == endingSceneName)
        {
            transform.root.gameObject.SetActive(false);
            Debug.Log($"[CoinManager] {scene.name} 로드 완료, Global UI 숨김.");
        }
        else
        {
            transform.root.gameObject.SetActive(true);
            Debug.Log("[CoinManager] 새로운 씬 로드 완료, Global UI 활성화.");
        }

        RefreshCoinUIAndNotify();
    }

    void Start()
    {
        RefreshCoinUIAndNotify();
        if (coinText != null)
            Debug.Log($"[CoinManager] 초기 자산: {FormatMoneyKorean(currentCoin)}");
    }

    // ✅ 코인 변경 공통 처리
    private void ApplyCoinChanged()
    {
        UpdateCoinDisplay();
        OnCoinChanged?.Invoke(currentCoin);

        // 잔액 변경 시마다 게임 오버(마이너스) 체크
        CheckForGameOver();
    }

    public void RefreshCoinUIAndNotify()
    {
        UpdateCoinDisplay();
        OnCoinChanged?.Invoke(currentCoin);
    }

    // 돈 쓰기 (구매)
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

    // 돈 벌기/잃기 (음수 amount 가능)
    public void AddCoin(long amount)
    {
        currentCoin += amount;
        ApplyCoinChanged();
    }

    public void AddCoin(int amount) => AddCoin((long)amount);
    public long GetCurrentCoin() => currentCoin;

    // ✅ 게임 오버 체크 및 카운트다운 시작 로직
    private void CheckForGameOver()
    {
        // 자산이 마이너스이고, 이미 카운트다운 중이 아닐 때
        if (currentCoin < 0 && !isCountingDown)
        {
            isCountingDown = true;
            Debug.Log("[CoinManager] 파산 위기! 카운트다운 시작.");
            StartCoroutine(CountdownRoutine());
        }
        // 자산이 다시 0 이상으로 회복되었을 때
        else if (currentCoin >= 0 && isCountingDown)
        {
            isCountingDown = false;
            StopAllCoroutines(); // 카운트다운 중지
            if (countdownText != null) countdownText.gameObject.SetActive(false); // 텍스트 끄기
            Debug.Log("[CoinManager] 자산 회복! 카운트다운 중지.");
        }
    }

    // ✅ 5초 카운트다운 코루틴
    private IEnumerator CountdownRoutine()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = "경고!";
        }

        // 5초부터 1초까지 카운트
        for (int i = 5; i > 0; i--)
        {
            if (countdownText != null)
            {
                countdownText.text = $"<color=red>{i}</color>";
            }
            yield return new WaitForSeconds(1f);
        }

        // 카운트다운 끝 -> 씬 전환
        if (countdownText != null) countdownText.text = "<color=red>GAME OVER</color>";
        yield return new WaitForSeconds(1f); // "GAME OVER" 문구 1초 보여줌

        Debug.Log($"[CoinManager] {endingSceneName}으로 이동합니다.");
        SceneManager.LoadScene(endingSceneName);
    }

    private void UpdateCoinDisplay()
    {
        if (coinText == null) return;
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

    public string FormatMoneyKorean(long money)
    {
        if (money == 0) return "0원";

        bool neg = money < 0;
        long absMoney = System.Math.Abs(money); // 절대값 변환

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
        return neg ? "-" + result : result; // 마이너스 기호 처리
    }
}