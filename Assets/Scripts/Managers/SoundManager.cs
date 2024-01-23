/*
 * Class for managing playing all related sounds.
 * 
 * @author Evan
 * @version January 23
 */

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

    /**
     * Plays a sound clip with the specified audio ID using the default audio source.
     * @param audioID The ID of the audio clip to play.
     */
    public void PlayAudio(int audioID)
    {
        audioSource.PlayOneShot(sounds[audioID]);
    }
    /**
     * Plays a sound clip with the specified audio ID using the provided audio source.
     * @param audioID The ID of the audio clip to play.
     * @param source The audio source to play the sound from.
     */
    public void PlayAudio(int audioID, AudioSource source)
    {
        source.PlayOneShot(sounds[audioID]);
    }
    /**
     * Plays the music clip with the specified music ID, stopping any currently playing music.
     * @param musicID The ID of the music clip to play.
     */
    public void PlayMusic(int musicID)
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        musicSource.clip = music[musicID];
        musicSource.Play();

    }

    /**
     * Stops the currently playing music.
     */
    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }
}
