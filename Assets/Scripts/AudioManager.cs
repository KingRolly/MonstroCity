using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// Plays and manages all audio for the game
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip buttonHoverSFX;
    [SerializeField] private AudioClip bgm;

    // Uses singleton design pattern
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        PlaySound(bgm, transform, 0.2f);
    }

    /// <summary>
    /// Play a given audio clip at given volume
    /// </summary>
    /// <param name="audioClip">Audio clip to play</param>
    /// <param name="spawnTransform">Transform of caller</param>
    /// <param name="volume">Volume to play audio clip at</param>
    public void PlaySound(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        // Spawn game object to play sound and assign audio clip accordingly
        AudioSource audioSource = Instantiate(audioPlayer, spawnTransform.position, Quaternion.identity);
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.Play();

        // Destroy clip after finished playing
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    /// <summary>
    /// Play button click sound
    /// </summary>
    public void PlayButtonClickSound()
    {
        // Spawn game object to play sound and assign audio clip accordingly
        PlaySound(buttonClickSFX, transform, 1f);
    }

    /// <summary>
    /// Play button hover sound
    /// </summary>
    public void PlayButtonHoverSound()
    {
        // Spawn game object to play sound and assign audio clip accordingly
        PlaySound(buttonHoverSFX, this.transform, 1f);
    }

    /// <summary>
    /// Set the master volume to given level
    /// </summary>
    /// <param name="level">Level to set volume to</param>
    public void SetMasterVolume(float level)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level) * 20);
        // We use this formula because decibels work on a logarithmic scale
    }

    /// <summary>
    /// Set the soundFX volume to given level
    /// </summary>
    /// <param name="level">Level to set volume to</param>
    public void SetSoundFXVolume(float level)
    {
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level) * 20);
        // We use this formula because decibels work on a logarithmic scale
    }

    /// <summary>
    /// Set the music volume to given level
    /// </summary>
    /// <param name="level">Level to set volume to</param>
    public void SetMusicVolume(float level)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level) * 20);
        // We use this formula because decibels work on a logarithmic scale
    }
}
