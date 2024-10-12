using System;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour
{
    [SerializeField] private Button unPauseButton;
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button restartBtn;
    public event EventHandler UnPauseGame;
    public event EventHandler SettingGame;
    private void Start()
    {
        gameObject.SetActive(false);
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

        });
        restartBtn.onClick.AddListener(() =>
        {

        });
        GameManagerUI.Instance.CallGamePauseUI += GameManagerUI_CallGamePauseUI;
    }

    private void GameManagerUI_CallGamePauseUI(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
