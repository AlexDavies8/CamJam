using System.Collections;
using System.Collections.Generic;
using MultiState;
using UnityEngine;

public class JumpState : PlayerState
{
    public JumpState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state) {}
    
    public float JumpTimer { get; set; }

    private float _fallGravity;

    public override void OnEnter()
    {
        JumpTimer = Settings.maxJumpTime;
        _fallGravity = State.motor.Gravity;
        State.motor.Gravity = Settings.jumpGravity;

        State.motor.Velocity = new Vector2(State.motor.Velocity.x, Settings.jumpVelocity);
        
        State.movementSmoothing = Settings.movementSmoothing * Settings.jumpMovementSmoothingMultiplier;
        
        State.animator.Play(Settings.jumpAnimation, 0);
    }
    
    public override void Tick()
    {
        if (!Input.GetKey(KeyCode.Space)) JumpTimer = 0f;
        JumpTimer -= Time.deltaTime;
    }

    public override void OnExit()
    {
        State.motor.Gravity = _fallGravity;

        State.movementSmoothing = Settings.movementSmoothing;
    }
}
