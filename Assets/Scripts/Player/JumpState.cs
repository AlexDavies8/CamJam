using System.Collections;
using System.Collections.Generic;
using MultiState;
using UnityEngine;

public class JumpState : IState
{
    public float JumpTimer { get; set; }
    
    private PlayerMotor _motor;
    
    public float jumpVelocity;
    public float jumpGravity;
    public float maxJumpTime;

    private float _fallGravity;
    
    public JumpState(PlayerMotor motor)
    {
        _motor = motor;
    }

    public void OnEnter()
    {
        JumpTimer = maxJumpTime;
        _fallGravity = _motor.Gravity;
        _motor.Gravity = jumpGravity;

        _motor.Velocity = new Vector2(_motor.Velocity.x, jumpVelocity);
    }
    
    public void Tick()
    {
        if (!Input.GetKey(KeyCode.Space)) JumpTimer = 0f;
        JumpTimer -= Time.deltaTime;
    }

    public void OnExit()
    {
        _motor.Gravity = _fallGravity;
    }
}
