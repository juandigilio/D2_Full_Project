using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PrayBehaviour : MonoBehaviour
{
    private Player player;
    private MovementBehaviour movementBehaviour;

    public static event Action OnAnimationPraying;
    public static event Action OnActivateQuest;

    private void Awake()
    {
        player = GetComponent<Player>();
        movementBehaviour = GetComponent<MovementBehaviour>();
    }

    public void Pray(InputAction.CallbackContext callbackContext)
    {
        if (movementBehaviour.IsGrounded() && !player.IsAnimating())
        {
            //Debug.Log("Pray!!!");
            player.IsAnimating(true);
            OnAnimationPraying?.Invoke();
        }
    }

    public void PrayFinished()
    {
        player.IsAnimating(false);
        Debug.Log("pray finished!!!!");

        OnActivateQuest?.Invoke();
    }
}