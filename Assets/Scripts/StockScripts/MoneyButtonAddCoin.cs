using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class MoneyButtonAddCoin : MonoBehaviour
{
    [Tooltip("추가할 금액(코인)")]
    public long amount = 100_000_000L;

    [Tooltip("한 번만 동작시키고 비활성화할지")]
    public bool oneShot = false;

    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnSelectEntered);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnSelectEntered);
    }

    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (CoinManager.Instance == null)
        {
            Debug.LogWarning("[MoneyButton] CoinManager.Instance 가 null입니다.");
            return;
        }

        CoinManager.Instance.AddCoin(amount);
        Debug.Log($"[MoneyButton] +{amount}");

        if (oneShot)
            interactable.enabled = false;
    }
}
