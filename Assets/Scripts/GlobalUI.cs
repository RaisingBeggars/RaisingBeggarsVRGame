using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalUI : MonoBehaviour
{
    private static GlobalUI instance;

    // 씬 전환 버튼 (StreetScene에서만 활성화)
    public GameObject sceneChangeButton;

    [Header("VR UI 설정")]
    // 카메라 기준 위치 (앞으로 0.5m, 약간 위로 0.35m)
    [SerializeField] private Vector3 vrLocalPosition = new Vector3(0.2f, 0.15f, 0.5f);
    // 크기 (10배 확대)
    [SerializeField] private Vector3 vrLocalScale = new Vector3(0.1f, 0.1f, 0.1f);

    // UI가 카메라를 따라다니는 속도 (높을수록 빠름, 0이면 즉시 이동)
    [SerializeField] private float followSpeed = 10f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // 가장 중요: UI를 최상위 루트로 유지해야 DontDestroyOnLoad가 작동함
            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        // World Space 렌더 모드 강제 적용
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            canvas.renderMode = RenderMode.WorldSpace;
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 시 버튼 활성화/비활성화
        if (sceneChangeButton != null)
        {
            sceneChangeButton.SetActive(scene.name == "StreetScene");
        }
    }

    // LateUpdate는 모든 카메라 움직임이 끝난 후 호출되므로 UI 떨림 방지에 좋음
    void LateUpdate()
    {
        // 매 프레임 Main Camera를 찾습니다. (씬이 바뀌어도 항상 찾음)
        Camera mainCam = Camera.main;

        if (mainCam != null)
        {
            Transform camTransform = mainCam.transform;

            // 1. 목표 위치 계산: 카메라 위치에서 설정된 오프셋만큼 떨어진 곳
            // TransformPoint는 로컬 좌표를 월드 좌표로 변환
            Vector3 targetPosition = camTransform.TransformPoint(vrLocalPosition);

            // 2. 목표 회전 계산: 카메라와 같은 방향을 바라보게 함
            Quaternion targetRotation = camTransform.rotation;

            // 3. 부드럽게 이동 (선택 사항: 즉시 이동하려면 Lerp 대신 바로 대입)
            if (followSpeed > 0)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * followSpeed);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * followSpeed);
            }
            else
            {
                transform.position = targetPosition;
                transform.rotation = targetRotation;
            }

            // 4. 스케일 고정 (혹시 변경될 경우를 대비)
            transform.localScale = vrLocalScale;
        }
    }
}