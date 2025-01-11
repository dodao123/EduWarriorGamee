using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource MusicAudioSource;
    public AudioSource vfxAudioSource;

    public AudioClip musicClip;
    public AudioClip walk;
    public AudioClip ThrowKnife;
    public AudioClip GetItem;
    public AudioClip hurt;
    public AudioClip Jumping;

    void Start()
    {
        MusicAudioSource.clip = musicClip;
        MusicAudioSource.volume = 0.1f; // Set music volume to 0.3 (adjust as needed)
        MusicAudioSource.Play();
    }

    public void PlaySFX(AudioClip sfxClip)
    {
        vfxAudioSource.volume = 2.0f; // Set SFX volume to 1.0 (adjust as needed)
        vfxAudioSource.PlayOneShot(sfxClip);
    }

    public void TestPlaySFX()
    {
        PlaySFX(GetItem);
    }
}
