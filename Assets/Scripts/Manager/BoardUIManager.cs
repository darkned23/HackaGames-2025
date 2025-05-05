using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Añadido para usar Slider

public class BoardUIManager : MonoBehaviour
{
    [SerializeField] private HandController handController;
    [SerializeField] private BoardGameManager boardGameManager;
    [SerializeField] private Slider scoreSlider;
    [SerializeField] private TMP_Text timerText;

    void OnEnable()
    {
        Subscribe();
        SetSliderMaxValue();
        UpdateScore();
        UpdateTimer();
    }

    void OnDisable()
    {
        Unsubscribe();
    }

    private void SetSliderMaxValue()
    {
        if (scoreSlider != null && boardGameManager != null)
            scoreSlider.maxValue = boardGameManager.ScoreToWin;
    }

    private void Subscribe()
    {
        if (handController != null)
            handController.OnScoreChanged += UpdateScoreText;
        if (boardGameManager != null)
            boardGameManager.OnTimerChanged += UpdateTimerText;
    }

    private void Unsubscribe()
    {
        if (handController != null)
            handController.OnScoreChanged -= UpdateScoreText;
        if (boardGameManager != null)
            boardGameManager.OnTimerChanged -= UpdateTimerText;
    }

    private void UpdateScore()
    {
        if (handController != null)
            UpdateScoreText(handController.Score);
    }

    private void UpdateTimer()
    {
        if (boardGameManager != null)
            UpdateTimerText(boardGameManager.TimeRemaining);
    }

    private void UpdateScoreText(int newScore)
    {
        if (scoreSlider != null && boardGameManager != null)
        {
            scoreSlider.maxValue = boardGameManager.ScoreToWin;
            scoreSlider.value = Mathf.Clamp(newScore, 0, boardGameManager.ScoreToWin);
        }
    }

    private void UpdateTimerText(float newTime)
    {
        if (timerText != null)
        {
            int seconds = Mathf.Max(0, Mathf.CeilToInt(newTime));
            timerText.text = $"Tiempo restante: {seconds}s";
        }
    }

    // Reinicia la escena actual
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        Time.timeScale = 1f;
    }

    // Carga una escena por índice
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);

        Time.timeScale = 1f;
    }
}
