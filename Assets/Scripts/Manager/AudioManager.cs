using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip[] musicClips;
    public AudioClip[] sfxClips;

    [Header("Escenas especiales para música")]
    public int[] specialSceneIndexes;
    private int lastSceneIndex = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(UnityEngine.SceneManagement.Scene oldScene, UnityEngine.SceneManagement.Scene newScene)
    {
        int newSceneIndex = newScene.buildIndex;
        bool isSpecial = false;
        for (int i = 0; i < specialSceneIndexes.Length; i++)
        {
            if (specialSceneIndexes[i] == newSceneIndex)
            {
                isSpecial = true;
                break;
            }
        }
        bool wasSpecial = false;
        for (int i = 0; i < specialSceneIndexes.Length; i++)
        {
            if (specialSceneIndexes[i] == lastSceneIndex)
            {
                wasSpecial = true;
                break;
            }
        }
        if (isSpecial)
        {
            // Reproduce el siguiente musicClip si existe
            if (musicClips != null && musicClips.Length > 1)
            {
                musicSource.clip = musicClips[1];
                musicSource.Play();
            }
        }
        else if (wasSpecial)
        {
            // Si antes estaba en una escena especial y ahora no, vuelve al primer musicClip
            if (musicClips != null && musicClips.Length > 0)
            {
                musicSource.clip = musicClips[0];
                musicSource.Play();
            }
        }
        lastSceneIndex = newSceneIndex;
    }

    private void Start()
    {
        // Si el musicSource ya tiene un clip asignado, lo reproduce
        if (musicSource != null && musicSource.clip != null)
        {
            musicSource.Play();
        }
        // Si no tiene clip pero hay clips en la lista, asigna y reproduce el primero
        else if (musicSource != null && musicClips != null && musicClips.Length > 0)
        {
            musicSource.clip = musicClips[0];
            musicSource.Play();
        }
        // Si no hay clip asignado ni clips en la lista, no hace nada
    }

    public void PlayMusic(int clipIndex)
    {
        if (musicSource == null || musicClips == null || musicClips.Length == 0) return;
        if (clipIndex < 0 || clipIndex >= musicClips.Length) return;
        musicSource.clip = musicClips[clipIndex];
        musicSource.Play();
    }

    public void PauseMusic()
    {
        if (musicSource != null)
            musicSource.Pause();
    }

    public void PlaySFX(int clipIndex)
    {
        if (sfxSource == null || sfxClips == null || sfxClips.Length == 0) return;
        if (clipIndex < 0 || clipIndex >= sfxClips.Length) return;
        sfxSource.PlayOneShot(sfxClips[clipIndex]);
    }
}