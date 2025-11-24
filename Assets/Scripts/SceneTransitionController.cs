using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransitionController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.7f;
    public string sceneToLoad;

    public void OnButtonPressed()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeAndLoad()
    {
        // Fade Out
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, t / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneToLoad);

        // Fade In
        t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeImage.color = new Color(0, 0, 0, 1 - (t / fadeDuration));
            yield return null;
        }
    }
}
