using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISoundSetting
{
    public static SoundManager Instance;
    private AudioSource gameAudioSource;
    [Range(0, 1)]
    [SerializeField] private float volumeSound;
    public enum Sound
    {
        GunHit,
        GunShoot,
        EnemyExplosion
    }
    private Dictionary<Sound, AudioClip> audioClipDictionary;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        gameAudioSource = GetComponent<AudioSource>();
        audioClipDictionary = new Dictionary<Sound, AudioClip>();
        foreach (Sound sound in Enum.GetValues(typeof(Sound)))
        {
            audioClipDictionary[sound] = Resources.Load<AudioClip>(sound.ToString());
        }
        volumeSound = PlayerPrefs.GetFloat("volumeSound", volumeSound);
    }
    private void Start()
    {
        
    }
    public void PlaySound(Sound sound)
    {
        gameAudioSource.PlayOneShot(audioClipDictionary[sound], volumeSound);
    }

    public void Increase()
    {
        volumeSound += .1f;
        volumeSound = Mathf.Clamp01(volumeSound);
        gameAudioSource.volume = volumeSound;
    }

    public void Decrease()
    {
        volumeSound -= .1f;
        volumeSound = Mathf.Clamp01(volumeSound);
        gameAudioSource.volume = volumeSound;
    }

    public int volumeNormalize()
    {
        PlayerPrefs.SetFloat("volumeSound", volumeSound);
        return Mathf.FloorToInt(volumeSound * 10);
    }
}
