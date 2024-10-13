using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour, ISoundSetting
{
    public static MusicManager Instance;
    [Range(0, 1)]
    [SerializeField] private float volumeMusic;
    private AudioSource gameAudioSource;
    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        gameAudioSource = GetComponent<AudioSource>();
        volumeMusic = PlayerPrefs.GetFloat("volumeMusic", volumeMusic);
        gameAudioSource.volume = volumeMusic;
    }
    public void Increase()
    {
        volumeMusic += .1f;
        volumeMusic = Mathf.Clamp01(volumeMusic);
        gameAudioSource.volume = volumeMusic;
    }
    public void Decrease()
    {
        volumeMusic -= .1f;
        volumeMusic = Mathf.Clamp01(volumeMusic);
        gameAudioSource.volume = volumeMusic;
    }
    public int volumeNormalize()
    {
        PlayerPrefs.SetFloat("volumeMusic", volumeMusic);
        return Mathf.FloorToInt((volumeMusic / 1) * 10);
    }
}
