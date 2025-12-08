using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSceneSpawnPoint : MonoBehaviour
{
    void Start()
    {
        // 현재 씬에서 XROrigin 하나 찾기 (BootstrapScene에서 DontDestroyOnLoad 된 XR Rig)
        var rig = FindAnyObjectByType<XROrigin>();
        if (rig == null)
        {
            Debug.LogWarning("XRSceneSpawnPoint: XROrigin not found in scene.");
            return;
        }

        // XR 리그 전체를 이 오브젝트 위치/회전으로 이동
        rig.transform.position = transform.position;
        rig.transform.rotation = transform.rotation;
    }
}
