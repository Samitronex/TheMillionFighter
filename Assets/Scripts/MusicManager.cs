using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;
    public AudioClip backgroundMusic;

    [Header("Sliders")]
    [SerializeField] private Slider musicSlider;  // Slider para música
    [SerializeField] private Slider effectsSlider; // Slider para efectos de sonido

    [Header("Audio Sources for Effects")]
    [SerializeField] private List<AudioSource> effectsAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }

        // Listener para el slider de música
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(musicSlider.value); });
        }

        // Listener para el slider de efectos
        if (effectsSlider != null)
        {
            effectsSlider.onValueChanged.AddListener(delegate { SetEffectsVolume(effectsSlider.value); });
        }
    }

    public static void SetMusicVolume(float volume)
    {
        if (Instance?.audioSource != null)
        {
            Instance.audioSource.volume = volume;
        }
    }

    public void SetEffectsVolume(float volume)
    {
        // Ajustar el volumen de todas las fuentes de efectos
        foreach (var source in effectsAudioSources)
        {
            if (source != null)
            {
                source.volume = volume;
            }
        }
    }

    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioSource == null) return;

        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }

        if (resetSong)
        {
            audioSource.Stop();
        }

        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PauseBackgroundMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
}
