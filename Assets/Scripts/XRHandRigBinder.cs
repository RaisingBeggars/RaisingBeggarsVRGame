// 전역으로 핸드 제스쳐 트래킹 연결용 스크립트

using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRHandRigBinder : MonoBehaviour
{
    [SerializeField] private MonoBehaviour target; // XRHandTrackingEvents 등
    [SerializeField] private string xrOriginFieldName = "xrOrigin";
    [SerializeField] private string xrCameraFieldName = "xrCamera";

    private void Awake()
    {
        var xrOrigin = FindAnyObjectByType<XROrigin>();
        var cam = xrOrigin != null && xrOrigin.Camera != null ? xrOrigin.Camera : Camera.main;

        if (target == null) return;

        var type = target.GetType();

        var originField = type.GetField(xrOriginFieldName,
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (originField != null)
            originField.SetValue(target, xrOrigin);

        var camField = type.GetField(xrCameraFieldName,
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);
        if (camField != null)
            camField.SetValue(target, cam);
    }
}

