using MultiState;
using UnityEngine;
using UnityEngine.Events;
using USync;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    [SerializeField] private PlayerControllerSettings _settings;

    public UnityEvent OnDie;
    
    [Header("Synced Properties")]
    public Sync<float> _lightIntensity = new(1f);
    public Sync<float> _health = new(1f);

    private PlayerControllerState _state;

    private StateMachine _stateMachine;

    private void Awake()
    {
        _health.Value = _settings.maxHealth;
        
        _state = new();
        _state.motor = GetComponent<PlayerMotor>();
        _state.animator = _animator;
        _state.movementSmoothing = _settings.movementSmoothing;
        
        SetupStateMachine();
    }

    private void SetupStateMachine()
    {
        _stateMachine = new StateMachine();

        var moveState = new MoveState(_settings, _state);
        //var attackState = new AttackState(_settings, _state);
        
        //_stateMachine.AddTransition(attackState, moveState, () => attackState.Completed);
       // _stateMachine.AddTransition(moveState, attackState, () => _state.attackInput);

        _stateMachine.SetState(moveState);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _state.jumpBufferTimer = _settings.jumpBufferTime;
        if (_state.motor.OnGround) _state.coyoteTimer = _settings.coyoteTime;

        if (Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Z)) _state.attackInput = true;
        
        _state.horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        _stateMachine.Tick();
        
        _state.jumpBufferTimer -= Time.deltaTime;
        _state.coyoteTimer -= Time.deltaTime;

        _lightIntensity.Value = Mathf.Lerp( _settings.minIntensity, _settings.maxIntensity, Mathf.PerlinNoise1D(Time.time * 5f));
    }

    public void TakeDamage(float amount)
    {
        GameManager.Instance.GetGlobalComponent<AudioManager>().PlaySound("PlayerHurt");
        _health.Value = Mathf.Clamp(_health.Value - amount, 0, _settings.maxHealth);
        if (amount > 0) GameManager.Instance.GetGlobalComponent<ScreenShaker>().AddTrauma(Mathf.Pow(amount / _settings.maxHealth, 0.2f) * 0.5f);
        if (_health.Value <= 0) OnDie.Invoke();
    }
}

public class PlayerControllerState
{
    public PlayerMotor motor;
    public Animator animator;
    
    public float horizontalInput;
    public bool attackInput;
    
    public float coyoteTimer;
    public float jumpBufferTimer;

    public float movementSmoothing;
}
