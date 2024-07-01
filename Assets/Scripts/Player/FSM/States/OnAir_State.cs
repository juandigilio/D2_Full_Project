using UnityEngine;


public class OnAir_State : Base_state
{
    public override Base_state Enter(Player player)
    {
        if (!player.MovementBehaviour().IsGrounded())
        {
            return this;
        }
        else
        {
            Debug.LogWarning("Couldn't enter becouse is still grounded");
            return null;
        }
    }

    public override void Update(Base_state currentState, Player player, StateManager stateManager)
    {
        if (player.MovementBehaviour().IsGrounded())
        {
            Exit(currentState, stateManager);
        }
    }

    public override void FixedUpdate(Base_state currentState, Player player)
    {
        if (player.MovementBehaviour().IsGrounded())
        {
            player.MovementBehaviour().Move();
        }
    }

    public override void Exit(Base_state currentState, StateManager stateManager)
    {
        stateManager.CheckCurrentState();
    }
}
