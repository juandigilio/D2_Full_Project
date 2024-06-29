using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class CustomSceneManager : MonoBehaviour
{
    private static PlayerInput playerInput;


    private void Start()
    {
        playerInput = InputManager.instance.PlayerInput;

        LoadSceneAsync("MainMenu");
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
        if (scene == "MainMenu")
        {
            playerInput.SwitchCurrentActionMap("Paused");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }
}
