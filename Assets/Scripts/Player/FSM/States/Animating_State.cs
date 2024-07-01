using UnityEngine;


public class Animating_State : Base_state
{
    public override Base_state Enter(Player player)
    {
        if (player.IsAnimating())
        {
            return this;
        }
        else
        {
            Debug.LogWarning("Couldn't enter becouse isn't animating");
            return null;
        }
    }

    public override void Update(Base_state currentState, Player player)
    {
        if (!player.IsAnimating())
        {
            Exit(currentState);
        }
    }

    public override void FixedUpdate(Base_state currentState, Player player)
    {
        return;
    }

    public override void Exit(Base_state currentState)
    {
        currentState = null;
    }
}
