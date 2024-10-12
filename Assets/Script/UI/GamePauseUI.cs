using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button unPauseButton;
    private Button pauseButton;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button restartBtn;
    public event EventHandler UnPauseGame;
    public event EventHandler SettingGame;
    public event EventHandler OnClickGamePauseBtn;
    private void Start()
    {
        gameObject.SetActive(false);
        pauseButton = GameObject.Find("PauseBtn").GetComponent<Button>();
        pauseButton.onClick.AddListener(() =>
        {
            OnClickGamePauseBtn?.Invoke(this, EventArgs.Empty);
        });
        unPauseButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            UnPauseGame?.Invoke(this, EventArgs.Empty);
        });
        settingBtn.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
            SettingGame?.Invoke(this, EventArgs.Empty);
        });
        mainMenuBtn.onClick.AddListener(() =>
        {
            LoadSceneManager.Load(LoadSceneManager.Scene.MainMenuScene);
        });
        restartBtn.onClick.AddListener(() =>
        {
            LoadSceneManager.Load(LoadSceneManager.Scene.GameScene);
        });
        GameManagerUI.Instance.OnGamePauseUI += GameManagerUI_CallGamePauseUI;
    }

    private void GameManagerUI_CallGamePauseUI(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
