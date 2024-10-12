using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerUI : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private GamePauseUI gamePauseUI;
    public static GameManagerUI Instance;
    public event EventHandler OnGameOverUI;
    public event EventHandler OnGamePauseUI;
    public event EventHandler OnGameManagerUnPauseGame;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameOver += GameManager_OnGameOver;
        gamePauseUI.UnPauseGame += GamePauseUI_UnPauseGame;
        gamePauseUI.SettingGame += GamePauseUI_SettingGame;
        gamePauseUI.OnClickGamePauseBtn += GamePauseUI_OnClickGamePauseBtn;
    }

    private void GamePauseUI_OnClickGamePauseBtn(object sender, EventArgs e)
    {
        OnGamePauseUI?.Invoke(this, EventArgs.Empty);
    }

    private void GamePauseUI_SettingGame(object sender, EventArgs e)
    {
        throw new NotImplementedException();
    }

    private void GamePauseUI_UnPauseGame(object sender, EventArgs e)
    {
        OnGameManagerUnPauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void GameManager_OnGameOver(object sender, System.EventArgs e)
    {
        OnGameOverUI?.Invoke(this, EventArgs.Empty);
    }
}
