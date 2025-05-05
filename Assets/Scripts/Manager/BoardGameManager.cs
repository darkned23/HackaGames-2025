using UnityEngine;

public class BoardGameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private HandController handController;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject panelHUB;
    [SerializeField] private AnimatedMouseFollower mouseFollower;

    [Header("Condiciones de juego")]
    [SerializeField] private int scoreToWin;
    [SerializeField] private float timeToLose;
    [SerializeField] private float endGameTimeScale = 0.2f;

    public float TimeRemaining => timer;
    public int ScoreToWin => scoreToWin;

    public delegate void TimerChanged(float newTime);
    public event TimerChanged OnTimerChanged; // Evento para notificar cambios de tiempo

    private bool gameEnded = false;
    private float timer;

    void Start()
    {
        timer = timeToLose;
        Subscribe();
        SetPanels(false, false);
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        OnTimerChanged?.Invoke(timer); // Notificar cambio de tiempo

        if (timer <= 0f)
            EndGame(false);
    }

    private void OnScoreChanged(int newScore)
    {
        if (gameEnded) return;
        if (newScore >= scoreToWin)
            EndGame(true);
    }

    private void EndGame(bool win)
    {
        gameEnded = true;
        SetPanels(win, !win);

        // Mostrar el cursor
        Cursor.visible = true;

        // Desactivar HandController y AnimatedMouseFollower
        if (handController != null)
            handController.enabled = false;
        if (mouseFollower != null)
            mouseFollower.enabled = false;

        // Desactivar el HUB
        if (panelHUB != null)
            panelHUB.SetActive(false);

        // Relentizar el tiempo
        Time.timeScale = endGameTimeScale;
    }

    private void SetPanels(bool showWin, bool showLose)
    {
        if (winPanel != null) winPanel.SetActive(showWin);
        if (losePanel != null) losePanel.SetActive(showLose);
    }

    private void Subscribe()
    {
        if (handController != null)
            handController.OnScoreChanged += OnScoreChanged;
    }

    private void Unsubscribe()
    {
        if (handController != null)
            handController.OnScoreChanged -= OnScoreChanged;
    }

    private void OnDestroy()
    {
        Unsubscribe();
    }
}