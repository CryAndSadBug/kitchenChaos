using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MusicManager : MonoBehaviour
{
    private string PLAYER_PREFS_Music_VOLUME = "musicVolume";

    public static MusicManager Instance;

    private AudioSource audioSource;

    private float volume = .3f;

    private void Awake()
    {

        Instance = this;

        audioSource = GetComponent<AudioSource>();

        PlayerPrefs.GetFloat(PLAYER_PREFS_Music_VOLUME, .3f);

        audioSource.volume = volume;
    }

    public void ChangeVolume()
    {
        volume += .1f;

        if (volume > 1f)
        {
            volume = 0f;
        }

        audioSource.volume = volume;

        PlayerPrefs.SetFloat(PLAYER_PREFS_Music_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
