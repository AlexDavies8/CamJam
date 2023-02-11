using UnityEngine;

public class WalkState : PlayerState
{
    public WalkState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public override void OnEnter()
    {
        State.animator.Play(Settings.walkAnimation);
    }
}
