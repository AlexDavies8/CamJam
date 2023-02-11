using MultiState;

public class IdleState : PlayerState
{
    public IdleState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public override void OnEnter()
    {
        State.animator.Play(Settings.idleAnimation);
    }
}