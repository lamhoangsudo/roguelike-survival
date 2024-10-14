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
        Play,
        GameOver
    }
    public GameStatus status {  get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        status = GameStatus.Play;
        HandlerGameStatus(GameStatus.Play);
        Player.OnPlayerDie += Player_OnPlayerDie;
        GameManagerUI.Instance.OnGamePauseUI += GameManagerUI_OnGamePause;
        GameManagerUI.Instance.OnGameManagerUnPauseGame += Instance_OnGameManagerUnPauseGame;
    }

    private void Instance_OnGameManagerUnPauseGame(object sender, EventArgs e)
    {
        HandlerGameStatus(GameStatus.Play);
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
        status = gameStatus;
        switch (gameStatus)
        {
            case GameStatus.Pause:
                OnGamePause?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
                break;
            case GameStatus.Play:
                Time.timeScale = 1;
                break;
            case GameStatus.GameOver:
                OnGameOver?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
                break;
        }
    }
    private void OnDestroy()
    {
        Player.OnPlayerDie -= Player_OnPlayerDie;
    }
}
