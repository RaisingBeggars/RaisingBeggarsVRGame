using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneButtons : MonoBehaviour
{
    public void GoStreet()
    {
        SceneManager.LoadScene("StreetScene");
    }

    public void GoStock()
    {
        SceneManager.LoadScene("StockSceneFIX");
    }

    public void GoShop()
    {
        SceneManager.LoadScene("StreetSceneShop");  // 씬 파일 이름과 완전히 동일하게
    }

}
