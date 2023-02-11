using UnityEngine;
using UnityEngine.Serialization;

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

    [Header("Animations")]
    public string idleAnimation = "Idle";
    public string walkAnimation = "Walk";
    public string jumpAnimation = "Jump";
    public string fallAnimation = "Fall";
    public string attackAnimation = "Attack";
    public float flipDirectionTime = 0.2f;

    [FormerlySerializedAs("_minIntensity")] [Header("Fire Light")]
    public float minIntensity = 0.8f;
    [FormerlySerializedAs("_maxIntensity")] public float maxIntensity = 1.2f;
}