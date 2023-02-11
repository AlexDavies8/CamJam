using MultiState;

public class PlayerState : IState
{
    protected PlayerControllerSettings Settings { get; }
    protected PlayerControllerState State { get; }
    
    public PlayerState(PlayerControllerSettings settings, PlayerControllerState state)
    {
        Settings = settings;
        State = state;
    }
    
    public virtual void Tick() {}

    public virtual void OnEnter() {}

    public virtual void OnExit() {}
}
