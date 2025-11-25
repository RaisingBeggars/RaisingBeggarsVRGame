using UnityEngine;

public class StockDetailPanel : MonoBehaviour
{
    // Left 전체(아무것도 선택 안 했을 때는 꺼두는 루트)
    public GameObject leftRoot;

    // Left 안의 회사별 패널들 (A, B, C, D, T)
    public GameObject[] companyPanels;

    // 현재 선택된 회사 인덱스 (나중에 Middle 패널에서 써먹을 수 있음)
    public int CurrentIndex { get; private set; } = -1;

    void Start()
    {
        HideAll();
    }

    public void HideAll()
    {
        // 회사 패널 전부 끄기
        if (companyPanels != null)
        {
            foreach (var p in companyPanels)
            {
                if (p != null) p.SetActive(false);
            }
        }

        // Left 전체 끄기
        if (leftRoot != null)
            leftRoot.SetActive(false);

        CurrentIndex = -1;
    }

    // FrontLeft에서 버튼/토글이 눌렸을 때 호출
    public void ShowCompany(int index)
    {
        if (companyPanels == null || index < 0 || index >= companyPanels.Length)
            return;

        if (leftRoot != null)
            leftRoot.SetActive(true);

        // ★ 여기서 "선택된 하나만 켜고 나머지는 전부 끄는" 게 핵심
        for (int i = 0; i < companyPanels.Length; i++)
        {
            if (companyPanels[i] != null)
                companyPanels[i].SetActive(i == index);
        }

        CurrentIndex = index;
    }
}
