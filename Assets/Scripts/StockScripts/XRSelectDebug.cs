using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRBaseInteractable))]
public class XRInteractableDebug : MonoBehaviour
{
    XRBaseInteractable it;

    void Awake()
    {
        it = GetComponent<XRBaseInteractable>();
    }

    void OnEnable()
    {
        it.hoverEntered.AddListener(OnHoverEntered);
        it.selectEntered.AddListener(OnSelectEntered);
    }

    void OnDisable()
    {
        it.hoverEntered.RemoveListener(OnHoverEntered);
        it.selectEntered.RemoveListener(OnSelectEntered);
    }

    void OnHoverEntered(HoverEnterEventArgs _)
        => Debug.Log($"[XR] HoverEntered: {name}");

    void OnSelectEntered(SelectEnterEventArgs _)
        => Debug.Log($"[XR] SelectEntered: {name}");
}
