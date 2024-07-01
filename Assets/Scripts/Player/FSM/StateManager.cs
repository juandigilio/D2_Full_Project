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

        PrayBehaviour.OnAnimationPraying += SetAnimating;
        PrayBehaviour.OnActivateQuest += AnimatingFinished;
    }

    private void Update()
    {
        //CheckCurrentState();
        Debug.Log("current state: " + currentState);

        if (currentState != null)
        {
            currentState.Update(currentState, player, this);
        }
    }

    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate(currentState, player);
        }
    }

    public void CheckCurrentState()
    {
        currentState = null;

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

    private void SetAnimating()
    {
        currentState = Animating;
    }

    private void AnimatingFinished()
    {
        currentState = OnFloor;
    }
}
