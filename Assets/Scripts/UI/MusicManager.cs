using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public AudioClip menuMusic;
    public AudioClip gameplayMusic;
    public AudioClip bossMusic;   // <-- NUEVA MUSICA

    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        float v = PlayerPrefs.GetFloat("Volumen", 0f);
        audioMixer.SetFloat("Volumen", v);

        PlayMenuMusic();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            PlayMenuMusic();
        }
        else if (scene.name == "Level-3")   // <-- ESCENA DEL BOSS
        {
            PlayBossMusic();
        }
        else
        {
            PlayGameplayMusic();
        }
    }

    void PlayMenuMusic()
    {
        if (audioSource.clip == menuMusic) return;
        audioSource.clip = menuMusic;
        audioSource.Play();
    }

    void PlayGameplayMusic()
    {
        if (audioSource.clip == gameplayMusic) return;
        audioSource.clip = gameplayMusic;
        audioSource.Play();
    }

    void PlayBossMusic()   // <-- NUEVA FUNCIÓN
    {
        if (audioSource.clip == bossMusic) return;
        audioSource.clip = bossMusic;
        audioSource.Play();
    }
}
