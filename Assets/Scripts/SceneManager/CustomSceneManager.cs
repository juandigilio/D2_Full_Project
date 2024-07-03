using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections.Generic;

struct SceneStruct
{
    public string sceneName;
    public string actionName;

    public static implicit operator SceneStruct(SceneActionMap other)
    {
        SceneStruct result = new SceneStruct();
        result.sceneName = other.sceneName;
        result.actionName = other.actionName;

        return result;
    }
}

public static class CustomSceneManager
{
    private static SceneStruct mainScene;
    private static SceneStruct mainMenu;
    private static List<SceneStruct> scenesPool = new List<SceneStruct>();
    private static SceneStruct winingScene;

    private static int index = 0;

    /// <summary>
    /// Sets up the scenes pool based on the provided scene dictionary.
    /// </summary>
    /// <param name="sceneDictionary">List of SceneActionMap defining scenes and actions.</param>
    public static void SetScenes(SceneActionMap main, SceneActionMap menu, List<SceneActionMap> sceneDictionary, SceneActionMap win)
    {
        mainScene = main;
        mainMenu = menu;

        foreach (SceneActionMap scene in sceneDictionary)
        {
            SceneStruct newScene = scene;

            scenesPool.Add(newScene);
        }

        winingScene = win;
    }

    /// <summary>
    /// Loads the next scene asynchronously from the scenes pool.
    /// </summary>
    public static void LoadNextSceneAsync()
    {
        SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);

        index++;

        SceneManager.LoadSceneAsync(scenesPool[index].sceneName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Loads a specific scene by name from the scenes pool.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public static void LoadScene(string sceneName)
    {
        for (int i = 0; i < scenesPool.Count; i++)
        {
            if (sceneName == scenesPool[i].sceneName)
            {
                if (index != 0)
                {
                    SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);
                    index = 0;
                }

                SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                index = i;
                break;
            }
        }
    }

    public static void ResetLevel()
    {
        SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);
        SceneManager.LoadSceneAsync(scenesPool[index].sceneName, LoadSceneMode.Additive);
    }
    
    public static void LoadWiningScene()
    {
        SceneManager.UnloadSceneAsync(scenesPool[index].sceneName);
        index = 0;
        SceneManager.LoadSceneAsync(winingScene.sceneName, LoadSceneMode.Additive);
    }

    public static void LoadMainMenu()
    {
        LoadScene(mainMenu.sceneName);
    }

    public static void ResetGame()
    {
        SceneManager.UnloadSceneAsync(winingScene.sceneName);
        scenesPool.Clear();
        SceneManager.LoadSceneAsync(mainScene.sceneName);
    }
}
