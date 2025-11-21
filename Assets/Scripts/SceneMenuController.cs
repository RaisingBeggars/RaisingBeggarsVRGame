using UnityEngine;
using UnityEngine.UI;

public class SceneMenuController : MonoBehaviour
{
    public GameObject sceneMenuPanel;
    public SceneTransitionController transition;

    // ST 버튼에서 호출
    public void OnSTButtonPressed()
    {
        sceneMenuPanel.SetActive(!sceneMenuPanel.activeSelf);
    }

    // 개별 Scene 버튼들에서 호출
    public void LoadStreet()
    {
        Debug.Log("LoadStreet 버튼 눌림");
        transition.sceneToLoad = "StreetScene";
        transition.OnButtonPressed();
    }

    public void LoadStock()
    {
        Debug.Log("LoadStock 버튼 눌림");
        transition.sceneToLoad = "StockScene";
        transition.OnButtonPressed();
    }

    public void LoadRiver()
    {
        Debug.Log("LoadRiver 버튼 눌림");
        transition.sceneToLoad = "RiverScene";
        transition.OnButtonPressed();
    }
}
