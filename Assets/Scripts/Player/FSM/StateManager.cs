using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Player player;
    Base_state currentState;
    OnFloor_State OnFloor;
    OnAir_State OnAir;
    Animating_State Animating;

    private void Awake()
    {
        player = GetComponent<Player>();

        OnFloor = new OnFloor_State();
        OnAir = new OnAir_State();
        Animating = new Animating_State();

        if (player == null)
        {
            Debug.Log("Player not found");
        }

        currentState = OnFloor;

        Debug.Log("current state: " + currentState);
    }

    private void Update()
    {
        CheckCurrentState();

        if (currentState != null)
        {
            currentState.Update(currentState, player);
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate(currentState, player);
        }
    }

    private void CheckCurrentState()
    {
        if (currentState == null)
        {
            currentState = Animating.Enter(player);

            if (currentState == null)
            {
                currentState = OnFloor.Enter(player);

                if (currentState == null)
                {
                    currentState = OnAir.Enter(player);

                    if (currentState == null)
                    {
                        Debug.LogError("Cant enter at any state!!!!!!!!!!");
                    }
                }
            }
        }
    }
}
