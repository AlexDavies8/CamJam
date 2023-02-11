using System;
using MultiState;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerControllerSettings _settings;
    
    private PlayerControllerState _state;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _state = new();
        _state.motor = GetComponent<PlayerMotor>();
        
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        _stateMachine = new StateMachine();

        var moveState = new MoveState(_settings, _state);
        
        _stateMachine.SetState(moveState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _state.jumpBufferTimer = _settings.jumpBufferTime;
        if (_state.motor.OnGround) _state.coyoteTimer = _settings.coyoteTime;
        
        _state.horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        _stateMachine.Tick();
        
        _state.jumpBufferTimer -= Time.deltaTime;
        _state.coyoteTimer -= Time.deltaTime;
    }
}

public class PlayerControllerState
{
    public PlayerMotor motor;
    public float horizontalInput;
    
    public float coyoteTimer;
    public float jumpBufferTimer;
}
