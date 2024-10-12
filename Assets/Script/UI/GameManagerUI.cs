using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private GamePauseUI gamePauseUI;
    [SerializeField] private Button pauseUI;
    public static GameManagerUI Instance;
    public event EventHandler CallGameOverUI;
    public event EventHandler CallGamePauseUI;
    public event EventHandler OnClickGamePauseBtn;
    public event EventHandler OnGameManagerUnPauseGame;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        pauseUI.onClick.AddListener(() =>
        {
            OnClickGamePauseBtn?.Invoke(this, EventArgs.Empty);
        });
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        GameManager.Instance.OnGamePause += GameManager_OnGamePause;
        gamePauseUI.UnPauseGame += GamePauseUI_UnPauseGame;
        gamePauseUI.SettingGame += GamePauseUI_SettingGame;
    }

    private void GamePauseUI_SettingGame(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void GamePauseUI_UnPauseGame(object sender, EventArgs e)
    {
        OnGameManagerUnPauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void GameManager_OnGamePause(object sender, EventArgs e)
    {
        CallGamePauseUI?.Invoke(this, EventArgs.Empty);
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        CallGameOverUI?.Invoke(this, EventArgs.Empty);
    }
}
