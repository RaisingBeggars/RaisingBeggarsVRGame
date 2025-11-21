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
        transition.sceneToLoad = "StreetScene";
        transition.OnButtonPressed();
    }

    public void LoadStock()
    {
        transition.sceneToLoad = "StockScene";
        transition.OnButtonPressed();
    }

    public void LoadRiver()
    {
        transition.sceneToLoad = "RiverScene";
        transition.OnButtonPressed();
    }
}
