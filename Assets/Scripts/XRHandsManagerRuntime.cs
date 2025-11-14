using UnityEngine;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Management;

/// <summary>
/// XR Hands Subsystem을 자동으로 시작하고 유지하는 간단한 매니저.
/// 일부 Unity 버전에서는 XRHandsManager 컴포넌트가 AddComponent에 표시되지 않기 때문에
/// 이 스크립트를 대신 사용합니다.
/// </summary>
public class XRHandsManagerRuntime : MonoBehaviour
{
    private XRHandSubsystem handSubsystem;

    void Awake()
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader == null)
        {
            Debug.LogError("❌ activeLoader is NULL! OpenXR not initialized.");
        }
        else
        {
            handSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();
            Debug.Log(handSubsystem != null
                ? "✅ XRHandSubsystem successfully loaded in Awake()"
                : "⚠️ XRHandSubsystem is NULL in Awake()");
        }
    }

    void OnEnable()
    {
        if (handSubsystem == null)
        {
            var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
            handSubsystem = loader?.GetLoadedSubsystem<XRHandSubsystem>();
        }

        if (handSubsystem != null)
        {
            handSubsystem.Start();
            Debug.Log("✅ XRHandSubsystem started.");
        }
        else
        {
            Debug.LogError("❌ OnEnable() could not get XRHandSubsystem!");
        }
    }


    void OnDisable()
    {
        if (handSubsystem != null && handSubsystem.running)
            handSubsystem.Stop();
    }

    void OnDestroy()
    {
        if (handSubsystem != null)
        {
            handSubsystem.Stop();
            handSubsystem = null;
        }
    }
    void Update()
{
    // Subsystem이 존재하지 않으면 다시 시도
    if (handSubsystem == null)
    {
        var loader = XRGeneralSettings.Instance?.Manager?.activeLoader;
        if (loader != null)
            handSubsystem = loader.GetLoadedSubsystem<XRHandSubsystem>();

        if (handSubsystem == null)
        {
            Debug.LogWarning("❌ XRHandSubsystem not found yet.");
            return;
        }
    }

    // Subsystem이 실행 중인지 체크
    if (!handSubsystem.running)
    {
        Debug.LogWarning("⚠️ XRHandSubsystem is not running.");
        return;
    }

    // 왼손 추적 여부 확인
    if (handSubsystem.leftHand.isTracked)
    {
        var leftPalm = handSubsystem.leftHand.GetJoint(XRHandJointID.Palm);
        if (leftPalm.TryGetPose(out Pose poseL))
            Debug.Log($"🖐 Left hand tracked at position: {poseL.position}");
    }
    else
    {
        Debug.Log("❌ Left hand not tracked.");
    }

    // 오른손 추적 여부 확인
    if (handSubsystem.rightHand.isTracked)
    {
        var rightPalm = handSubsystem.rightHand.GetJoint(XRHandJointID.Palm);
        if (rightPalm.TryGetPose(out Pose poseR))
            Debug.Log($"🖐 Right hand tracked at position: {poseR.position}");
    }
    else
    {
        Debug.Log("❌ Right hand not tracked.");
    }
}

}
