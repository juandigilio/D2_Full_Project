using UnityEngine;


public class OnFloor_State : Base_state
{
    public override Base_state Enter(Player player)
    {
        if (player.MovementBehaviour().IsGrounded())
        {
            return this;
        }
        else
        {
            Debug.LogWarning("Couldn't enter becouse its not grounded");

            return null;
        }
    }

    public override void Update(Base_state currentState, Player player)
    {
        if (!player.MovementBehaviour().IsGrounded())
        {
            Exit(currentState);
        }
    }

    public override void FixedUpdate(Base_state currentState, Player player)
    {
        player.MovementBehaviour().Move();
    }

    public override void Exit(Base_state currentState)
    {
        currentState = null;
    }
}
