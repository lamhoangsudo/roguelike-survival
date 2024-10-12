using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public event EventHandler OnGameOver;
    public event EventHandler OnGamePause;
    public enum GameStatus
    {
        Pause,
        UnPause,
        GameOver
    }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        Player.OnPlayerDie += Player_OnPlayerDie;
        GameManagerUI.Instance.OnClickGamePauseBtn += GameManagerUI_OnGamePause;
        GameManagerUI.Instance.OnGameManagerUnPauseGame += Instance_OnGameManagerUnPauseGame;
    }

    private void Instance_OnGameManagerUnPauseGame(object sender, EventArgs e)
    {
        HandlerGameStatus(GameStatus.UnPause);
    }

    private void GameManagerUI_OnGamePause(object sender, EventArgs e)
    {
        HandlerGameStatus(GameStatus.Pause);
    }

    private void Player_OnPlayerDie(object sender, System.EventArgs e)
    {
        HandlerGameStatus(GameStatus.GameOver);
    }
    private void HandlerGameStatus(GameStatus gameStatus)
    {
        switch (gameStatus)
        {
            case GameStatus.Pause:
                OnGamePause?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
                break;
            case GameStatus.UnPause:
                Time.timeScale = 1;
                break;
            case GameStatus.GameOver:
                OnGameOver?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
                break;
        }
    }
}
