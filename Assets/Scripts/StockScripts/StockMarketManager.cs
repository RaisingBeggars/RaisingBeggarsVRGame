using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StockId
{
    A,
    B,
    C,
    D,
    T
}

[System.Serializable]
public class StockData
{
    public StockId id;
    public string displayName;
    public float currentPrice;
    public bool alwaysUp;
    public List<float> history = new List<float>();
}

public class StockMarketManager : MonoBehaviour
{
    public static StockMarketManager Instance { get; private set; }

    [Tooltip("게임에 사용할 주식 목록")]
    public StockData[] stocks;

    [Tooltip("주가 갱신 간격(초)")]
    public float updateIntervalSeconds = 30f;

    [Tooltip("한 번 갱신할 때 최대 변동 퍼센트 (0.1 = ±10%)")]
    public float maxChangePercent = 0.1f;

    [Tooltip("주가가 이 값 아래로는 내려가지 않도록 최소값 설정")]
    public float minPrice = 100f;

    [Tooltip("각 주식별로 최대 몇 개까지 기록할지")]
    public int maxHistoryCount = 10;

    // 다음 갱신까지 남은 시간을 계산하기 위한 기준 시간
    private float nextUpdateTime;

    public Action OnPricesUpdated;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        foreach (var s in stocks)
        {
            if (s.history == null)
                s.history = new List<float>();

            // ★ 히스토리가 비어 있으면 시작 가격을 하나 넣어준다
            if (s.history.Count == 0)
                s.history.Add(s.currentPrice);
        }

        nextUpdateTime = Time.time + updateIntervalSeconds;
        StartCoroutine(UpdatePricesLoop());
    }

    private IEnumerator UpdatePricesLoop()
    {
        var wait = new WaitForSeconds(updateIntervalSeconds);

        while (true)
        {
            yield return wait;

            UpdatePricesOnce();
            nextUpdateTime = Time.time + updateIntervalSeconds;
        }
    }

    private void UpdatePricesOnce()
    {
        foreach (var s in stocks)
        {
            float oldPrice = s.currentPrice;

            float deltaPercent;
            if (s.alwaysUp)
                deltaPercent = UnityEngine.Random.Range(0f, maxChangePercent);
            else
                deltaPercent = UnityEngine.Random.Range(-maxChangePercent, maxChangePercent);

            float newPrice = oldPrice * (1f + deltaPercent);
            if (newPrice < minPrice)
                newPrice = minPrice;

            s.currentPrice = newPrice;
            s.history.Add(newPrice);

            // ★ 중요: 처음 가격(history[0])은 항상 남기고,
            // 그 이후 변화(maxHistoryCount개)만 유지
            if (s.history.Count > maxHistoryCount + 1)
            {
                s.history.RemoveAt(1);   // index 1 = 가장 오래된 "변동" 값
            }
        }

        OnPricesUpdated?.Invoke();
    }


    public float GetPrice(StockId id)
    {
        foreach (var s in stocks)
        {
            if (s.id == id) return s.currentPrice;
        }
        Debug.LogWarning($"Stock {id} not found");
        return 0f;
    }

    public IReadOnlyList<float> GetHistory(StockId id)
    {
        foreach (var s in stocks)
        {
            if (s.id == id) return s.history;
        }
        return null;
    }

    // 다음 변동까지 남은 시간(초)
    public float GetRemainingSeconds()
    {
        return Mathf.Max(0f, nextUpdateTime - Time.time);
    }


    // 이름/데이터 찾기
        public StockData GetStock(StockId id)
    {
        foreach (var s in stocks)
            if (s.id == id) return s;

        return null;
    }

        public string GetDisplayName(StockId id)
    {
        foreach (var s in stocks)
            if (s.id == id)
                return string.IsNullOrEmpty(s.displayName) ? id.ToString() : s.displayName;

        return id.ToString();
    }

    public long GetPriceLong(StockId id)
    {
        return (long)Mathf.RoundToInt(GetPrice(id)); // 기존 GetPrice(float) 활용
    }

}
