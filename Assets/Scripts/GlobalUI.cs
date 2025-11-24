using UnityEngine;
using UnityEngine.SceneManagement; // SceneManager를 사용하기 위해 필요

public class GlobalUI : MonoBehaviour
{
    private static GlobalUI instance;

    // 씬 전환 버튼 (유니티 에디터에서 연결)
    public GameObject sceneChangeButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 씬 전환 이벤트에 OnSceneLoaded 함수 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 이 오브젝트가 파괴될 때 이벤트 리스너 해제
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (sceneChangeButton == null)
        {
            Debug.LogError("[GlobalUI] 씬 전환 버튼 오브젝트가 연결되지 않았습니다.");
            return;
        }

        // 현재 씬의 이름을 확인하여 버튼 활성화/비활성화 결정
        if (scene.name == "StreetScene")
        {
            // StreetScene일 때만 버튼을 활성화 (보이게 함)
            sceneChangeButton.SetActive(true);
            Debug.Log("[GlobalUI] StreetScene 로드: 씬 전환 버튼 활성화.");
        }
        else
        {
            // StreetScene이 아닐 때는 버튼을 비활성화 (숨김)
            sceneChangeButton.SetActive(false);
            Debug.Log($"[GlobalUI] {scene.name} 로드: 씬 전환 버튼 비활성화.");
        }
    }
}