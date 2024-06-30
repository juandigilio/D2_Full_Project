using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class CustomSceneManager : MonoBehaviour
{
    private static PlayerInput playerInput;


    private void Start()
    {
        playerInput = InputManager.instance.PlayerInput;

        LoadSceneAsync(SceneActionDictionary.GetScenesList()[0].sceneName);
    }

    public static void LoadSceneAsync(string scene)
    {
        SetInput(scene);

        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    public static void UnLoadSceneAsync(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
    }

    private static void SetInput(string scene)
    {
        if (scene == SceneActionDictionary.GetScenesList()[0].sceneName)
        {
            playerInput.SwitchCurrentActionMap(SceneActionDictionary.GetScenesList()[0].actionName);
        }
        else
        {
            playerInput.SwitchCurrentActionMap(SceneActionDictionary.GetScenesList()[1].actionName);
        }
    }
}
