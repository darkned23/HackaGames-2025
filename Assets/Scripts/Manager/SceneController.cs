using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    public Image fadeImage;
    public GameObject panelPause;
    public bool LoadOnStart = false;
    public int sceneIndexToLoadOnStart = 1;
    public float fadeDuration = 1.5f;
    public PlayerInput playerInput;

    void Start()
    {
        if (panelPause != null && playerInput != null)
        {
            panelPause.SetActive(false);
            playerInput.actions["Pause"].performed += ctx => PauseGame(ctx);
        }

        if (LoadOnStart)
        {
            LoadSceneByIndex(sceneIndexToLoadOnStart);
        }
    }
    public void LoadCurrentScene()
    {
        Time.timeScale = 1f;

        if (panelPause != null)
        {
            panelPause.SetActive(false);
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(FadeInAndLoadScene(currentIndex));
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        Time.timeScale = 1f;

        if (panelPause != null)
        {
            panelPause.SetActive(false);
        }

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

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (panelPause.activeSelf)
            {
                ResumeGame();
            }
            else
            {
                Time.timeScale = 0f;
                panelPause.SetActive(true);
            }
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        panelPause.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}
