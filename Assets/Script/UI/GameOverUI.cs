using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Button mainMenuBtn;
    [SerializeField] private Button restartBtn;
    private void Start()
    {
        gameObject.SetActive(false);
        mainMenuBtn.onClick.AddListener(() =>
        {

        });
        restartBtn.onClick.AddListener(() =>
        {

        });
        GameManagerUI.Instance.CallGameOverUI += GameManagerUI_CallGameOverUI;
    }

    private void GameManagerUI_CallGameOverUI(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);
    }
}
