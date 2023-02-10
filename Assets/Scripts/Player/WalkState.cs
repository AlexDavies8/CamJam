using System;
using System.Collections;
using System.Collections.Generic;
using MultiState;
using UnityEngine;

public class WalkState : IState
{
    private PlayerMotor _motor;

    public Func<float> horizontalInputGetter;
    public float movementSpeed = 5f;
    public float movementSmoothing = 2f;

    private float _moveVelocity = 0f;
    
    public WalkState(PlayerMotor motor)
    {
        _motor = motor;
    }

    public void OnEnter()
    {
        
    }
    
    public void Tick()
    {
        float horizontalInput = horizontalInputGetter();
        
        float target = horizontalInput * movementSpeed;
        float smoothFac = movementSmoothing * movementSpeed * Time.deltaTime;
        _moveVelocity += Mathf.Clamp(target - _moveVelocity, -smoothFac, smoothFac);
        
        Debug.Log(target);

        _motor.Velocity = new Vector2(_moveVelocity, _motor.Velocity.y);
    }

    public void OnExit()
    {
        
    }
}
