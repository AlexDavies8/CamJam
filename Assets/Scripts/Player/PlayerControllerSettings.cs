using UnityEngine;

[CreateAssetMenu(menuName = "Custom/PlayerControllerSettings")]
public class PlayerControllerSettings : ScriptableObject
{
    [Header("Movement")]
    public float movementSpeed = 5f;
    public float movementSmoothing = 2f;
    
    [Header("Jumping")]
    public float jumpBufferTime = 0.1f;
    public float coyoteTime = 0.1f;
    public float jumpVelocity = 1.0f;
    public float jumpGravity = 0.6f;
    public float maxJumpTime = 1.0f;
    public float jumpMovementSmoothingMultiplier = 0.5f;

    [Header("Falling")]
    public float impactScreenShake = 0.2f;
    public float fallMovementSmoothingMultiplier = 0.5f;
}