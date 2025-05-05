using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1.5f;

    public void LoadCurrentScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeInAndLoadScene(currentIndex));
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        StartCoroutine(FadeInAndLoadScene(sceneIndex));
    }

    private IEnumerator FadeInAndLoadScene(int sceneIndex)
    {
        float timer = 0f;
        Color color = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneIndex);
    }
}
