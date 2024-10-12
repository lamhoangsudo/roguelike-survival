using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Button exitlBtn;
    private void Start()
    {
        playBtn.onClick.AddListener(() =>
        {
            LoadSceneManager.Load(LoadSceneManager.Scene.GameScene);
        });
        exitlBtn.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}
