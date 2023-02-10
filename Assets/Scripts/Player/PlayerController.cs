using System;
using MultiState;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _movementSpeed = 5f;

    [SerializeField] private float _movementSmoothing = 2f;
    
    [Header("Jumping")]
    [SerializeField] private float _jumpBufferTime = 0.1f;
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _jumpVelocity = 1.0f;
    [SerializeField] private float _jumpGravity = 0.6f;
    [SerializeField] private float _maxJumpTime = 1.0f;

    private StateMachine _stateMachine;

    private PlayerMotor _motor;

    private float _jumpBuffer;
    private float _coyoteTimer;

    private float _horizontalInput;

    private void Awake()
    {
        _motor = GetComponent<PlayerMotor>();
        
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        _stateMachine = new StateMachine();
        
        // On Ground
        var groundedStateMachine = new StateMachine();
        var idleState = new IdleState();
        var walkState = new WalkState(_motor)
        {
            movementSpeed = _movementSpeed,
            movementSmoothing = _movementSmoothing,
            horizontalInputGetter = () => _horizontalInput
        };
        groundedStateMachine.AddTransition(idleState, walkState, () => Mathf.Abs(_horizontalInput) > 0.01f);
        groundedStateMachine.AddTransition(walkState, idleState, () => Mathf.Abs(_motor.Velocity.x) <= 0.01f && Mathf.Abs(_horizontalInput) <= 0.01f);
        groundedStateMachine.SetState(idleState);
        var groundedState = new SubMachineState(groundedStateMachine);
        
        // In Air
        var jumpState = new JumpState(_motor)
        {
            jumpGravity = _jumpGravity,
            jumpVelocity = _jumpVelocity,
            maxJumpTime = _maxJumpTime
        };
        var fallState = new FallState();
        //var climbState = new ClimbState();
        
        _stateMachine.AddTransition(groundedState, fallState, () => _coyoteTimer < 0f && !_motor.OnGround);
        _stateMachine.AddTransition(groundedState, jumpState, () => _coyoteTimer >= 0f && _jumpBuffer >= 0f);
        _stateMachine.AddTransition(fallState, groundedState, () => _motor.OnGround);
        _stateMachine.AddTransition(jumpState, fallState, () => jumpState.JumpTimer < 0f);
        
        _stateMachine.SetState(fallState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpBuffer = _jumpBufferTime;
        }
        if (_motor.OnGround) _coyoteTimer = _coyoteTime;
        
        _horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        _stateMachine.Tick();
        
        _jumpBuffer -= Time.deltaTime;
        _coyoteTimer -= Time.deltaTime;
    }
}
