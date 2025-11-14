using UnityEngine;

public class StockDetailPanel : MonoBehaviour
{
    // Left 전체(아무것도 안 골랐을 때는 이걸 통째로 꺼둠)
    public GameObject leftRoot;

    // 회사별 디테일 패널들 (APanel, BPanel, ...)
    public GameObject[] companyPanels;

    void Start()
    {
        HideAll();
    }

    void HideAll()
    {
        // 회사 패널 전부 끄기
        foreach (var panel in companyPanels)
        {
            if (panel != null)
                panel.SetActive(false);
        }

        // Left 전체 숨기기
        if (leftRoot != null)
            leftRoot.SetActive(false);
    }

    // FrontLeft에서 버튼이 눌렸을 때 호출할 함수
    public void ShowCompany(int index)
    {
        if (leftRoot == null || companyPanels == null) return;
        if (index < 0 || index >= companyPanels.Length) return;

        // Left 전체 보이기
        leftRoot.SetActive(true);

        // 해당 회사만 켜고 나머지는 끄기
        for (int i = 0; i < companyPanels.Length; i++)
        {
            if (companyPanels[i] != null)
                companyPanels[i].SetActive(i == index);
        }
    }
}
