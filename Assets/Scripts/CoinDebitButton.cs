using UnityEngine;
using UnityEngine.UI;

// 이 스크립트는 버튼 오브젝트에 붙입니다.
public class CoinDebitButton : MonoBehaviour
{
    // 차감할 금액을 Inspector에서 설정할 수 있도록 합니다.
    [SerializeField] private long debitAmount = -10000;

    // 버튼 컴포넌트를 참조합니다.
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();

        if (button == null)
        {
            Debug.LogError("CoinDebitButton 스크립트가 Button 컴포넌트가 없는 오브젝트에 붙어 있습니다!");
            return;
        }

        // 버튼 클릭 이벤트에 함수 연결
        button.onClick.AddListener(DebitCoin);
    }

    private void DebitCoin()
    {
        if (CoinManager.Instance != null)
        {
            // CoinManager의 AddCoin 함수를 호출하여 금액을 차감합니다 (음수 금액 전달).
            CoinManager.Instance.AddCoin(debitAmount);
            Debug.Log($"버튼 클릭: {debitAmount} 코인을 차감했습니다.");
        }
        else
        {
            Debug.LogError("CoinManager 인스턴스를 찾을 수 없습니다!");
        }
    }
}