//보유주식패널에 붙일 스크립트
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StockPortfolioPanel : MonoBehaviour
{
    [Header("Refs")]
    public Transform contentRoot;              // VerticalLayoutGroup 붙은 Content
    public StockHoldingRowView rowPrefab;      // Row 프리팹
    public GameObject emptyHint;               // “보유 없음” 텍스트

    private readonly List<StockHoldingRowView> spawned = new();
    private bool bound;

    private void OnEnable()
    {
        bound = false;
        StartCoroutine(BindWhenReady());
    }

    private void OnDisable()
    {
        Unbind();
        ClearRows();
    }

    private IEnumerator BindWhenReady()
    {
        // Awake/OnEnable 실행 순서가 불확정이라, Instance 준비될 때까지 1프레임씩 대기
        while (enabled && (StockPortfolioManager.Instance == null || StockMarketManager.Instance == null))
            yield return null;

        if (!enabled) yield break;

        Bind();
        Refresh();
    }

    private void Bind()
    {
        if (bound) return;

        var pm = StockPortfolioManager.Instance;
        var sm = StockMarketManager.Instance;

        if (pm != null) pm.OnPortfolioChanged += Refresh;
        if (sm != null) sm.OnPricesUpdated += Refresh;

        bound = true;
    }

    private void Unbind()
    {
        if (!bound) return;

        var pm = StockPortfolioManager.Instance;
        var sm = StockMarketManager.Instance;

        if (pm != null) pm.OnPortfolioChanged -= Refresh;
        if (sm != null) sm.OnPricesUpdated -= Refresh;

        bound = false;
    }

    public void Refresh()
{
    Debug.Log($"[PortfolioPanel] Refresh pm={(StockPortfolioManager.Instance!=null)} sm={(StockMarketManager.Instance!=null)}");

    ClearRows();

    var pm = StockPortfolioManager.Instance;
    var sm = StockMarketManager.Instance;

    if (pm == null || sm == null || contentRoot == null || rowPrefab == null)
    {
        if (emptyHint) emptyHint.SetActive(true);
        return;
    }

    var list = pm.GetHoldingsOwned(); // shares>0만
    if (emptyHint) emptyHint.SetActive(list.Count == 0);
    if (list.Count == 0) return;

    list.Sort((a, b) => a.id.CompareTo(b.id));

    foreach (var h in list)
    {
        // worldPositionStays=false
        var row = Instantiate(rowPrefab, contentRoot, false);
        spawned.Add(row);

        //RectTransform 스케일/좌표 정리
        var rt = row.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.localScale = Vector3.one;
            rt.anchoredPosition3D = Vector3.zero; // LayoutGroup이 있으면 어차피 다시 잡힘
        }
        else
        {
            row.transform.localScale = Vector3.one;
            row.transform.localPosition = Vector3.zero;
        }

        string name = sm.GetDisplayName(h.id);
        long cur = sm.GetPriceLong(h.id);

        row.Bind(name, h.shares, h.avgPrice, cur);
    }
}


    private void ClearRows()
    {
        for (int i = 0; i < spawned.Count; i++)
            if (spawned[i]) Destroy(spawned[i].gameObject);
        spawned.Clear();
    }
}
