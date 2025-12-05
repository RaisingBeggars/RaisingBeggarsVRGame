using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    public TMP_Text coinText;

    // 새로운 변수: 마이너스 시간 측정 로직
    [SerializeField] private float timeUntilSceneChange = 5f; // 씬 전환까지 걸리는 시간 (5초)
    private float timeInNegative; // 마이너스 상태가 지속된 시간
    private bool isNegative = false; // 현재 코인 상태가 마이너스인지 여부

    [SerializeField]
    private long currentCoin = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);

            // ⭐ 씬 로드 이벤트 등록 (Awake에서 등록해야 DontDestroyOnLoad 후에도 유지됨)
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(transform.root.gameObject);
        }
    }

    // ⭐ 오브젝트가 파괴될 때 이벤트 해제
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // ⭐ 씬 로드가 완료될 때마다 호출되는 함수
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Global Canvas를 포함하는 CoinManager의 루트 오브젝트 (transform.root.gameObject)를 제어합니다.

        if (scene.name == "RiverScene")
        {
            // RiverScene일 경우, Canvas를 포함하는 이 오브젝트를 비활성화 (숨기기)
            transform.root.gameObject.SetActive(false);
            Debug.Log("[CoinManager] RiverScene 로드 완료, Global UI 숨김.");
        }
        else
        {
            // RiverScene이 아닌 다른 씬일 경우, 다시 활성화 (예: StreetScene 등)
            transform.root.gameObject.SetActive(true);
            Debug.Log("[CoinManager] 새로운 씬 로드 완료, Global UI 활성화.");
        }

        // 씬 로드 후 UI가 초기화될 수 있으므로, 다시 한번 금액을 업데이트합니다.
        UpdateCoinDisplay();
    }

    void Start()
    {
        UpdateCoinDisplay();

        if (coinText != null)
        {
            Debug.Log($"[CoinManager] 초기 자산 설정 완료: {FormatMoneyKorean(currentCoin)}");
        }
    }

    void Update()
    {
        // 1. 코인 상태가 마이너스일 때만 시간 측정
        if (isNegative)
        {
            timeInNegative += Time.deltaTime;

            // 2. 5초가 초과되면 씬 전환
            if (timeInNegative >= timeUntilSceneChange)
            {
                Debug.Log("[CoinManager] 코인 부족 시간 초과 (5초), RiverScene으로 이동합니다.");
                LoadRiverScene();
            }
        }
    }

    // 코인 변경 시마다 상태를 확인하고 시간 측정을 시작/초기화하는 헬퍼 함수
    private void CheckCoinStatus()
    {
        // currentCoin이 음수이고, 아직 마이너스 상태로 인식되지 않았을 때
        if (currentCoin < 0 && !isNegative)
        {
            // 코인이 마이너스로 전환됨: 시간 측정 시작
            isNegative = true;
            timeInNegative = 0f; // 시간 초기화
            Debug.Log("[CoinManager] 코인 마이너스 감지, 5초 후 씬 전환 타이머 시작.");

            UpdateCoinDisplay();
        }
        // currentCoin이 0 이상이고, 이전에 마이너스 상태였을 때
        else if (currentCoin >= 0 && isNegative)
        {
            // 코인이 0 이상으로 회복됨: 시간 측정 중지 및 초기화
            isNegative = false;
            timeInNegative = 0f;
            Debug.Log("[CoinManager] 코인 회복 감지, 씬 전환 타이머 중지.");

            UpdateCoinDisplay();
        }
        else if (currentCoin < 0 && isNegative)
        {
            // 이미 마이너스 상태이지만, 금액만 변했을 경우 UI 업데이트
            UpdateCoinDisplay();
        }
    }

    // 씬 전환 함수
    private void LoadRiverScene()
    {
        SceneManager.LoadScene("RiverScene");
    }

    // 아이템 구매 로직: 돈이 부족하면 구매를 실패(false)시킴
    public bool TrySpend(long amount)
    {
        if (amount <= 0) return false;

        // 구매 전 잔액 확인: 돈이 부족하면 구매 실패
        if (currentCoin < amount)
        {
            Debug.Log("[CoinManager] 잔액 부족으로 구매 실패.");
            return false;
        }

        currentCoin -= amount;
        UpdateCoinDisplay();

        CheckCoinStatus();

        return true;
    }

    // 돈 추가/차감 로직: 다른 요인으로 인한 코인 변동에 사용되며 마이너스를 허용함
    public void AddCoin(long amount)
    {
        // amount가 음수일 경우 차감(패널티), 양수일 경우 추가
        currentCoin += amount;

        // 코인 변경 후 상태 확인 로직 추가 (마이너스 상태 감지)
        CheckCoinStatus();
    }

    // public void AddCoin(int amount) 오버로드
    public void AddCoin(int amount)
    {
        AddCoin((long)amount);
    }

    // --- 나머지 기존 함수는 그대로 유지됩니다. ---

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
        // 이 함수는 FormatMoneyKorean에서 절댓값으로 호출되므로, 음수 처리는 필요 없습니다.
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

    // ⭐ FormatMoneyKorean 함수 수정: 마이너스 금액 표시 기능 추가
    private string FormatMoneyKorean(long money)
    {
        if (money == 0) return "0원";

        bool isNegative = money < 0;
        // ⭐ money의 절댓값을 사용하여 포매팅 로직을 재사용합니다.
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

        string result = sb.ToString().Trim() + "원";

        // ⭐ 금액이 음수일 경우 최종 문자열 앞에 마이너스 기호를 추가합니다.
        if (isNegative)
        {
            return "-" + result;
        }

        return result;
    }
}