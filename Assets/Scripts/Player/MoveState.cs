
using MultiState;
using UnityEngine;

public class MoveState : PlayerState
{
    private StateMachine _stateMachine;

    private float _moveVelocity;

    public MoveState(PlayerControllerSettings settings, PlayerControllerState state) : base(settings, state)
    {
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        var jumpState = new JumpState(Settings, State);
        var fallState = new FallState(Settings, State);
        
        var idleState = new IdleState(Settings, State);
        var walkState = new WalkState(Settings, State);
        
        var groundedStateMachine = new StateMachine();
        groundedStateMachine.AddTransition(idleState, walkState, () => State.horizontalInput != 0f);
        groundedStateMachine.AddTransition(walkState, idleState, () => State.horizontalInput == 0f && State.motor.Velocity.x == 0f);
        
        var groundedState = new SubMachineState(groundedStateMachine);
        
        _stateMachine = new StateMachine();
        _stateMachine.AddTransition(groundedState, jumpState, () => State.jumpBufferTimer >= 0 && State.coyoteTimer >= 0);
        _stateMachine.AddTransition(jumpState, fallState, () => jumpState.JumpTimer < 0 || State.motor.Velocity.y < 0f);
        _stateMachine.AddTransition(groundedState, fallState, () => State.coyoteTimer < 0 && !State.motor.OnGround);
        _stateMachine.AddTransition(fallState, groundedState, () => State.motor.OnGround);
        
        _stateMachine.SetState(fallState);
    }

    public override void Tick()
    {
        _stateMachine.Tick();
        
        float target = State.horizontalInput * Settings.movementSpeed;
        float smoothFac = Settings.movementSmoothing * Settings.movementSpeed * Time.deltaTime;
        _moveVelocity += Mathf.Clamp(target - _moveVelocity, -smoothFac, smoothFac);

        State.motor.Velocity = new Vector2(_moveVelocity, State.motor.Velocity.y);
    }
}