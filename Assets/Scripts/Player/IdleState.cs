using MultiState;
using UnityEngine;

public class IdleState : PlayerState
{
    public IdleState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public override void OnEnter()
    {
        State.animator.Play(Settings.idleAnimation, 0);
    }
}