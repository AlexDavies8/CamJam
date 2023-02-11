using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [field: SerializeField] public float Gravity { get; set; }
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _maxVelocity = 30f;
    
    public Vector2 Velocity { get; set; }
    public float MaxVelocity => _maxVelocity;
    public bool OnGround { get; private set; }
    
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        UpdateOnGround();

        if (!OnGround) ApplyGravity();
        else if (Velocity.y <= 0) Velocity = new Vector2(Velocity.x, -0.1f);

        Velocity = Vector2.ClampMagnitude(Velocity, _maxVelocity);
        _rb.velocity = Velocity;
    }

    public void ApplyGravity()
    {
        Velocity += Vector2.down * (Gravity * Time.fixedDeltaTime);
    }

    private void UpdateOnGround()
    {
        var pos = _groundCheck.transform.position;
        var size = _groundCheck.transform.localScale;
        OnGround = Physics2D.OverlapArea(pos - size / 2, pos + size / 2, _groundLayer.value);
    }

    private void OnDrawGizmos()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_groundCheck.transform.position, _groundCheck.transform.localScale);
        }
    }
}
