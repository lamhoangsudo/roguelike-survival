using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingUI : MonoBehaviour
{
    [SerializeField] private GameObject soundSetting;
    [SerializeField] private Button backBtn;
    [SerializeField] private Button backSoundBtn;
    [SerializeField] private Button soundBtn;
    [SerializeField] private Button controlBtn;
    [SerializeField] private Button soundIncrease;
    [SerializeField] private Button soundDecrease;
    [SerializeField] private TextMeshProUGUI soundVolume;
    [SerializeField] private Button musicIncrease;
    [SerializeField] private Button musicDecrease;
    [SerializeField] private TextMeshProUGUI musicVolume;
    public event EventHandler OnCallGamePauseUI;
    private void Start()
    {
        gameObject.SetActive(false);
        soundSetting.SetActive(false);
        GameManagerUI.Instance.OnGameSettingUI += GameManagerUI_OnGameSettingUI;
        backBtn.onClick.AddListener(()=>
        {
            gameObject.SetActive(false);
            OnCallGamePauseUI?.Invoke(this, EventArgs.Empty);
        });
        soundBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            soundSetting.SetActive(true);
        });
        controlBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
        backSoundBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(true);
            soundSetting.SetActive(false);
        });
        soundIncrease.onClick.AddListener( ()=>
        {
            SoundManager.Instance.Increase();
            soundVolume.text = SoundManager.Instance.volumeNormalize().ToString();
        });
        soundDecrease.onClick.AddListener(() =>
        {
            SoundManager.Instance.Decrease();
            soundVolume.text = SoundManager.Instance.volumeNormalize().ToString();
        });
        musicIncrease.onClick.AddListener(() =>
        {
            MusicManager.Instance.Increase();
            musicVolume.text = MusicManager.Instance.volumeNormalize().ToString();
        });
        musicDecrease.onClick.AddListener(() =>
        {
            MusicManager.Instance.Decrease();
            musicVolume.text = MusicManager.Instance.volumeNormalize().ToString();
        });
        soundVolume.text = SoundManager.Instance.volumeNormalize().ToString();
        musicVolume.text = MusicManager.Instance.volumeNormalize().ToString();
    }

    private void GameManagerUI_OnGameSettingUI(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
