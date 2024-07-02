using System;
using UnityEngine;

public class CheatsManager : MonoBehaviour
{
    private Arch arch;
    public static event Action OnOpenDoor;

    private void Awake()
    {
        arch = GetComponent<Arch>();

        InputManager.OnOpenDoor += DoorCheat;
    }

    private void OnDisable()
    {
        InputManager.OnOpenDoor -= DoorCheat;
    }

    /// <summary>
    /// Handles the cheat action to open the door.
    /// </summary>
    private void DoorCheat()
    {
        OnOpenDoor?.Invoke();
    }
}
