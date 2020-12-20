using System.Collections;
using UnityEngine;

class AudioManager : MonoBehaviour
{
    #region Static Instance
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if(instance == null)
                {
                    instance = new GameObject("Spawned AudioManager", typeof(AudioManager)).GetComponent<AudioManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    #endregion

    #region Fields
    private AudioSource musicSource;
    private AudioSource musicSource2;
    private AudioSource sfxSource;

    private bool sourceOneIsPlaying;

    #endregion

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();
        sfxSource = gameObject.AddComponent<AudioSource>();

        musicSource.loop = true;
        musicSource2.loop = true;

    }

    public void PlayMusic(AudioClip musicClip)
    {
        AudioSource activeSource = (sourceOneIsPlaying) ? musicSource : musicSource2;

        activeSource.clip = musicClip;
        activeSource.Play();
    }
    public void PlayMusicWithFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (sourceOneIsPlaying) ? musicSource : musicSource2;
        StartCoroutine(UpdateMusicWithFade(activeSource, newClip, transitionTime));
    }
    public void PlayMusicWithCrossFade(AudioClip newClip, float transitionTime = 1.0f)
    {
        AudioSource activeSource = (sourceOneIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (sourceOneIsPlaying) ? musicSource2 : musicSource;

        sourceOneIsPlaying = !sourceOneIsPlaying;

        newSource.clip = newClip;
        newSource.Play();
        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, transitionTime));
    }

    private IEnumerator UpdateMusicWithCrossFade(AudioSource original, AudioSource newSource, float transitionTime)
    {
        float t = 0.0f;

        for (t = 0.0f ; t < transitionTime; t += Time.deltaTime)
        {
            original.volume = (1 - (t / transitionTime));
            newSource.volume = (t / transitionTime);
            yield return null;
        }

        original.Stop();
    }
    private IEnumerator UpdateMusicWithFade(AudioSource activeSource, AudioClip newClip, float transitionTime)
    {
        if (!activeSource.isPlaying)
            activeSource.Play();

        float t = 0.0f;

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            activeSource.volume = (PlayerPrefs.GetFloat("music_volume") - (t / transitionTime));
            yield return null;
        }

        activeSource.Stop();
        activeSource.clip = newClip;
        activeSource.Play();

        for (t = 0; t < transitionTime; t += Time.deltaTime)
        {
            if ((t / transitionTime) >= PlayerPrefs.GetFloat("music_volume"))
                continue;
            activeSource.volume = ((t / transitionTime));
            yield return null;
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("music_volume", volume);
        musicSource.volume = volume;
        musicSource2.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        sfxSource.PlayOneShot(sfxClip);
    }
    public void PlaySFX(AudioClip sfxClip, float volume)
    {
        sfxSource.PlayOneShot(sfxClip, volume);
    }
}
