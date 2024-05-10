using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource bgMusic;
    [SerializeField] private AudioSource collectSound;
    [SerializeField] private AudioSource fallsound;
    [SerializeField] private AudioSource putSound;
    [SerializeField] private AudioSource winSound;
    [SerializeField] private AudioSource losesound;

    private void Start()
    {
        PlayBGMusic();
    }

    public void PlayBGMusic()
    {
        bgMusic.Play();
    }

    public void PlayCollectSound()
    {
        collectSound.Play();
    }

    public void PlayFallSound()
    {
        fallsound.Play();
    }

    public void PlayPutSound()
    {
        putSound.Play();
    }

    public void PlayWinSound()
    {
        winSound.Play();
    }

    public void PlayLoseSound()
    {
        losesound.Play();
    }
}
