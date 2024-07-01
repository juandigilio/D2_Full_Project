public abstract class Base_state
{ 
    public abstract Base_state Enter(Player player);
    public abstract void Update(Base_state currentState, Player player);
    public abstract void FixedUpdate(Base_state currentState, Player player);
    public abstract void Exit(Base_state currentState);
}
