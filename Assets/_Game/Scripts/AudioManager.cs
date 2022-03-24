using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip _musicGame = null;
    public static AudioManager instance = null;

    AudioSource _audioSource;

    private void Awake()
    {
            if(_musicGame != null)
        {
            AudioManager.instance.PlaySong(_musicGame);
        }
    }

    public void PlaySong(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
