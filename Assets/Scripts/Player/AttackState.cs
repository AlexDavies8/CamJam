
using System.Collections;
using UnityEngine;

public class AttackState : PlayerState
{
    public AttackState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}

    public bool Completed => _timer < 0f;

    private float _timer;
    
    public override void OnEnter()
    {
        State.attackInput = false;
        State.animator.Play(Settings.attackAnimation);
        _timer = State.animator.GetAnimationClip(Settings.attackAnimation).length;
    }

    public override void Tick()
    {
        _timer -= Time.deltaTime;
    }
}