using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

struct SceneStruct
{
    public string sceneName;
    public string actionName;
}

public static class CustomSceneManager
{
    private static List<SceneStruct> scenesPool = new List<SceneStruct>();

    private static int index = 0;
    private static int totalScenes = 0;


    public static void SetScenes(List<SceneActionMap> sceneDictionary)
    {
        foreach (SceneActionMap scene in sceneDictionary)
        {
            SceneStruct newScene;
            newScene.sceneName = scene.sceneName;
            newScene.actionName = scene.actionName;

            scenesPool.Add(newScene);
            totalScenes++;
        }
    }

    public static void LoadNextSceneAsync()
    {
        SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);

        index++;

        //SetInput(scenesPool[index].actionName);

        SceneManager.LoadSceneAsync(scenesPool[index].sceneName, LoadSceneMode.Additive);
    }

    public static void LoadMainMenu()
    {
        if (index != 0)
        {
            SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);
            index = 0;
        }

        SceneManager.LoadSceneAsync(scenesPool[index].sceneName, LoadSceneMode.Additive);
    }
}
