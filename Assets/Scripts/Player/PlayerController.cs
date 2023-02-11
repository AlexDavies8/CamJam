using MultiState;
using UnityEngine;
using USync;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    public Sync<float> _lightIntensity = new(1f);
    
    [SerializeField] private Animator _animator;
    
    [SerializeField] private PlayerControllerSettings _settings;
    
    private PlayerControllerState _state;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _state = new();
        _state.motor = GetComponent<PlayerMotor>();
        _state.animator = _animator;
        
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

        _lightIntensity.Value = Mathf.PerlinNoise1D(Time.time * 5f) * 0.5f + 0.5f;
    }
}

public class PlayerControllerState
{
    public PlayerMotor motor;
    public Animator animator;
    
    public float horizontalInput;
    
    public float coyoteTimer;
    public float jumpBufferTimer;
}
