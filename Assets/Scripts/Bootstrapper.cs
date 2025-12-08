using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "StreetScene";

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);  // XR Rig + GlobalCanvas 전역 유지
    }

    private void Start()
    {
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Single);
    }
}
