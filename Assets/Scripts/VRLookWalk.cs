using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRLookWalk : MonoBehaviour
{
    public Transform vrCamera;            // set in inspector (child camera)
    public float toggleAngle = 30f;       // degrees down
    public float speed = 3f;              // units per second
    public float gravity = -9.81f;

    private CharacterController cc;
    private Vector3 velocity;

    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (vrCamera == null) return;

        // compute pitch in -180..180 range (negative when looking down)
        float pitch = vrCamera.localEulerAngles.x;
        if (pitch > 180f) pitch -= 360f;

        bool moveForward = pitch <= -Mathf.Abs(toggleAngle); // looking down past toggleAngle

        if (moveForward)
        {
            // forward relative to the camera's Y rotation only (so looking down doesn't tilt movement)
            Vector3 forward = Quaternion.Euler(0f, vrCamera.eulerAngles.y, 0f) * Vector3.forward;
            Vector3 move = forward * speed * Time.deltaTime;
            // preserve vertical velocity (gravity)
            move.y = velocity.y * Time.deltaTime;
            cc.Move(move);
        }
        else
        {
            // still apply gravity so we stick to ground
            cc.Move(Vector3.up * velocity.y * Time.deltaTime);
        }

        // gravity handling
        if (cc.isGrounded && velocity.y < 0f)
        {
            velocity.y = -1f; // small negative to keep grounded
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
    }
}
