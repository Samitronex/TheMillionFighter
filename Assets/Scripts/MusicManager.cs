// MusicManager
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

        AssignSliders();
    }

    private void AssignSliders()
    {
        if (musicSlider != null)
        {
            Debug.Log("Asignando slider de música.");
            musicSlider.onValueChanged.RemoveAllListeners();
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            musicSlider.value = audioSource != null ? audioSource.volume : 1f;
        }
        else
        {
            Debug.LogWarning("Slider de música no asignado en el MusicManager.");
        }

        if (effectsSlider != null)
        {
            Debug.Log("Asignando slider de efectos.");
            effectsSlider.onValueChanged.RemoveAllListeners();
            effectsSlider.onValueChanged.AddListener(SetEffectsVolume);

            if (effectsAudioSources.Count > 0 && effectsAudioSources[0] != null)
            {
                effectsSlider.value = effectsAudioSources[0].volume;
            }
        }
        else
        {
            Debug.LogWarning("Slider de efectos no asignado en el MusicManager.");
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
            Debug.Log($"Volumen de música ajustado a {volume}");
        }
    }

    public void SetEffectsVolume(float volume)
    {
        foreach (var source in effectsAudioSources)
        {
            if (source != null)
            {
                source.volume = volume;
                Debug.Log($"Ajustando volumen de efectos a {volume} para {source.name}");
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
