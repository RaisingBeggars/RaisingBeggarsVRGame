using UnityEngine;
using UnityEngine.SceneManagement;

public class PurchaseZone : MonoBehaviour
{
    [SerializeField] private string targetTag = "Buyable";

    // 빌딩의 가격 (1억 원)
    [SerializeField] private long buildingCost = 100000000;

    private bool isPurchased = false;

    private void OnTriggerEnter(Collider other)
    {
        // 이미 구입 처리 중이거나 CoinManager 인스턴스가 없는 경우 즉시 리턴
        if (isPurchased || CoinManager.Instance == null)
        {
            return;
        }

        // 1. 콜라이더의 루트 오브젝트(아이템)가 목표 태그("Buyable")를 가지고 있는지 확인
        if (other.attachedRigidbody != null && other.attachedRigidbody.gameObject.CompareTag(targetTag))
        {
            // 빌딩 아이템을 찾음
            GameObject building = other.attachedRigidbody.gameObject;

            // 2. CoinManager의 TrySpend 함수를 호출하여 구매를 시도합니다.
            if (CoinManager.Instance.TrySpend(buildingCost))
            {
                // **구매 성공 시**
                isPurchased = true;
                Debug.Log("빌딩 구매 성공! 1억 원이 차감되었습니다. 씬 전환을 준비합니다.");


                //  잠시 후 씬 전환 (2초 대기)
                Invoke("LoadClearScene", 2f);
            }
            else
            {
                // **구매 실패 시 (돈 부족)**
                Debug.Log($"[구매 실패] 돈이 부족합니다! 현재 코인: {CoinManager.Instance.GetCurrentCoin()}");
                // 아이템을 상자에 넣었지만 돈이 부족하므로 아무것도 하지 않고 대기합니다.
            }
        }
    }

    private void LoadClearScene()
    {
        SceneManager.LoadScene("ClearScene");
    }
}