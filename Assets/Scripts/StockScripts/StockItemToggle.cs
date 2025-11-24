using UnityEngine;
using UnityEngine.UI;   // Toggle 사용

public class StockItemToggle : MonoBehaviour
{
    // 회사 상세 패널 관리 스크립트
    public StockDetailPanel detailPanel;

    // 이 Item 이 몇 번째 회사인지 (0 = A, 1 = B, ...)
    public int companyIndex;

    void Awake()
    {
        // 같은 오브젝트에 붙어 있는 Toggle 가져오기
        Toggle toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleChanged);
        }
    }

    void OnDestroy()
    {
        // 씬에서 사라질 때 리스너 제거 (안전용)
        Toggle toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleChanged);
        }
    }

    void OnToggleChanged(bool isOn)
    {
        // 선택이 켜졌을 때만 동작
        if (!isOn) return;
        if (detailPanel != null)
        {
            detailPanel.ShowCompany(companyIndex);
        }
    }
}
