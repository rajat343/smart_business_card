using UnityEngine;

/// <summary>
/// VR locomotion system that moves the player forward when looking down
/// Uses head tracking to control movement direction
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class VRLookWalk : MonoBehaviour
{
    // Reference to the VR camera transform (set in inspector)
    public Transform vrCamera;
    
    // Angle threshold in degrees - looking down past this angle triggers movement
    public float lookDownThreshold = 30f;
    
    // Movement speed in units per second
    public float movementSpeed = 3f;
    
    // Gravity force applied to the character
    public float gravityForce = -9.81f;

    // Character controller component reference
    private CharacterController characterController;
    
    // Current velocity vector for gravity and movement
    private Vector3 currentVelocity;
    
    // Constant for grounding check
    private const float GROUNDED_VELOCITY = -1f;
    
    // Constant for pitch angle wrapping
    private const float ANGLE_WRAP_THRESHOLD = 180f;

    /// <summary>
    /// Initialize component references
    /// </summary>
    void Start()
    {
        // Cache the character controller component
        characterController = GetComponent<CharacterController>();
        
        // Initialize velocity
        currentVelocity = Vector3.zero;
    }

    /// <summary>
    /// Update movement and gravity each frame
    /// </summary>
    void Update()
    {
        // Early exit if camera reference is missing
        if (vrCamera == null) 
        {
            Debug.LogWarning("[VRLookWalk] VR Camera reference is not set!");
            return;
        }

        // Calculate the camera's pitch angle (looking up/down)
        float cameraPitch = CalculateCameraPitch();

        // Determine if player should move forward based on look angle
        bool shouldMoveForward = cameraPitch <= -Mathf.Abs(lookDownThreshold);

        if (shouldMoveForward)
        {
            // Calculate and apply forward movement
            ApplyForwardMovement();
        }
        else
        {
            // Apply gravity only (no horizontal movement)
            ApplyGravityOnly();
        }

        // Update gravity velocity
        UpdateGravity();
    }

    /// <summary>
    /// Calculate the camera's pitch angle in the range of -180 to 180
    /// </summary>
    /// <returns>Normalized pitch angle</returns>
    private float CalculateCameraPitch()
    {
        // Get the raw pitch from local euler angles
        float rawPitch = vrCamera.localEulerAngles.x;
        
        // Normalize to -180..180 range (negative when looking down)
        if (rawPitch > ANGLE_WRAP_THRESHOLD)
        {
            rawPitch -= 360f;
        }
        
        return rawPitch;
    }

    /// <summary>
    /// Apply forward movement relative to camera's horizontal rotation
    /// </summary>
    private void ApplyForwardMovement()
    {
        // Get forward direction relative to camera's Y rotation only
        // This prevents looking down from tilting the movement direction
        Quaternion horizontalRotation = Quaternion.Euler(0f, vrCamera.eulerAngles.y, 0f);
        Vector3 forwardDirection = horizontalRotation * Vector3.forward;
        
        // Calculate horizontal movement
        Vector3 horizontalMovement = forwardDirection * movementSpeed * Time.deltaTime;
        
        // Preserve vertical velocity from gravity
        horizontalMovement.y = currentVelocity.y * Time.deltaTime;
        
        // Apply the movement to character controller
        characterController.Move(horizontalMovement);
    }

    /// <summary>
    /// Apply gravity without horizontal movement
    /// </summary>
    private void ApplyGravityOnly()
    {
        // Apply only vertical movement to keep character grounded
        Vector3 verticalMovement = Vector3.up * currentVelocity.y * Time.deltaTime;
        characterController.Move(verticalMovement);
    }

    /// <summary>
    /// Update gravity velocity based on grounded state
    /// </summary>
    private void UpdateGravity()
    {
        // Check if character is on the ground
        if (characterController.isGrounded && currentVelocity.y < 0f)
        {
            // Keep a small negative velocity to ensure grounded state
            currentVelocity.y = GROUNDED_VELOCITY;
        }
        else
        {
            // Apply gravity acceleration
            currentVelocity.y += gravityForce * Time.deltaTime;
        }
    }
}