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

    /// <summary>
    /// Sets up the scenes pool based on the provided scene dictionary.
    /// </summary>
    /// <param name="sceneDictionary">List of SceneActionMap defining scenes and actions.</param>
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
}
