using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadSceneManager
{
    public enum Scene
    {
        MainMenuScene,
        LoadingScenes,
        GameScene
    }
    public static Scene targetScene;
    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(LoadSceneManager.Scene.LoadingScenes.ToString());
    }
    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
