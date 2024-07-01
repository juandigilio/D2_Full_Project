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

    public override void Update(Base_state currentState, Player player, StateManager stateManager)
    {
        Debug.Log("In animation state");
    }

    public override void FixedUpdate(Base_state currentState, Player player)
    {
        return;
    }

    public override void Exit(Base_state currentState, StateManager stateManager)
    {
        Debug.Log("exit animation state");
        stateManager.CheckCurrentState();
    }
}
