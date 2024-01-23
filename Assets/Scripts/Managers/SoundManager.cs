using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("References")]
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] private List<AudioClip> music;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayAudio(int audioID)
    {
        audioSource.PlayOneShot(sounds[audioID]);
    }

    public void PlayAudio(int audioID, AudioSource source)
    {
        source.PlayOneShot(sounds[audioID]);
    }

    public void PlayMusic(int musicID)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = music[musicID];
        musicSource.Play();

    }


    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
