using UnityEngine;

public class StateManager : MonoBehaviour
{
    private Player player;
    Base_state currentState;
    OnFloor_State OnFloor;
    OnAir_State OnAir;
    Animating_State Animating;

    /// <summary>
    /// Initializes states and subscribes to events.
    /// </summary>
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

        PrayBehaviour.OnAnimationPraying += SetAnimating;
        PrayBehaviour.OnActivateQuest += AnimatingFinished;
    }

    /// <summary>
    /// Updates the current state.
    /// </summary>
    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update(currentState, player, this);
            Debug.Log("state = active");
            Debug.Log("currentState" + currentState);
        }
        else
        {
            Debug.Log("state = null");
        }
    }

    /// <summary>
    /// FixedUpdate for physics-related updates in the current state.
    /// </summary>
    private void FixedUpdate()
    {
        if (currentState != null)
        {
            currentState.FixedUpdate(currentState, player);
        }
    }

    /// <summary>
    /// Checks and sets the current state.
    /// </summary>
    public void CheckCurrentState()
    {
        currentState = null;

        if (currentState == null)
        {
            currentState = OnFloor.Enter(player);

            if (currentState == null)
            {
                currentState = OnAir.Enter(player);
            }
        }
    }

    private void CheckNullState()
    {
        if (currentState == null)
        {
            currentState = OnFloor.Enter(player);

            if (currentState == null)
            {
                currentState = OnAir.Enter(player);
            }
        }
    }

    /// <summary>
    /// Sets the current state to Animating.
    /// </summary>
    private void SetAnimating()
    {
        currentState = Animating;
    }

    /// <summary>
    /// Sets the current state to OnFloor.
    /// </summary>
    private void AnimatingFinished()
    {
        currentState = OnFloor;
    }
}
