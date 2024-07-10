using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CheatsManager : MonoBehaviour
{
    [SerializeField] Altar altar;
    private PlayerInput playerInput;
    private InputManager inputManager;
    private Player player;

    private void Awake()
    {
        GameObject input = GameObject.FindGameObjectWithTag("InputManager");
        playerInput = input.GetComponent<PlayerInput>();
        inputManager = input.GetComponent<InputManager>();

        GameObject obj = GameObject.FindGameObjectWithTag("Player");
        player = obj.GetComponent<Player>();

        playerInput.SwitchCurrentActionMap(inputManager.playerAction);
        playerInput.currentActionMap.FindAction(inputManager.doorAction).started += OpenDoor;
    }

    private void OnDisable()
    {
        playerInput.SwitchCurrentActionMap(inputManager.playerAction);
        playerInput.currentActionMap.FindAction(inputManager.doorAction).started -= OpenDoor;

        player.Cheated(false);
    }

    /// <summary>
    /// Invokes the OnOpenDoor action.
    /// </summary>
    private void OpenDoor(InputAction.CallbackContext callbackContext)
    {
        if (!player.Cheated())
        {
            altar.CheatDoor();
            player.Cheated(true);
        }   
    }

    private void CollectAllCoins(InputAction.CallbackContext callbackContext)
    {

    }
}
