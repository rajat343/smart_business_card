using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the opening and closing animation of Lamborghini-style doors
/// Doors open when camera enters trigger zone and close when it exits
/// </summary>
public class LamboDoorBehaviour : MonoBehaviour 
{
    // Current rotation angle of the door
    private float currentRotationAngle = 0f;
    
    // Target rotation angle the door is moving towards
    private float targetRotationAngle = 0f;
    
    // Speed multiplier for door animation
    private const float DOOR_ANIMATION_SPEED = 3f;
    
    // Maximum open angle for the door
    private const float DOOR_OPEN_ANGLE = 60f;
    
    // Closed angle for the door
    private const float DOOR_CLOSED_ANGLE = 0f;
    
    // Tag to check for camera collision
    private const string CAMERA_TAG = "MainCamera";

    /// <summary>
    /// Update is called once per frame
    /// Smoothly interpolates door rotation towards target angle
    /// </summary>
    void Update() 
    {
        // Smoothly lerp current angle towards desired angle
        currentRotationAngle = Mathf.LerpAngle(
            currentRotationAngle, 
            targetRotationAngle, 
            Time.deltaTime * DOOR_ANIMATION_SPEED
        );
        
        // Apply the rotation to the door's local transform
        transform.localEulerAngles = new Vector3(currentRotationAngle, 0f, 0f);
    }

    /// <summary>
    /// Sets the target angle to open the door
    /// </summary>
    void OpenDoors() 
    {
        targetRotationAngle = DOOR_OPEN_ANGLE;
        Debug.Log("[LamboDoorBehaviour] Opening door to " + DOOR_OPEN_ANGLE + " degrees");
    }

    /// <summary>
    /// Sets the target angle to close the door
    /// </summary>
    void CloseDoors() 
    {
        targetRotationAngle = DOOR_CLOSED_ANGLE;
        Debug.Log("[LamboDoorBehaviour] Closing door to " + DOOR_CLOSED_ANGLE + " degrees");
    }

    /// <summary>
    /// Called when another collider enters the trigger zone
    /// Opens doors if the camera enters
    /// </summary>
    /// <param name="other">The collider that entered the trigger</param>
    private void OnTriggerEnter(Collider other) 
    {
        // Check if the entering collider is the main camera
        if (other.CompareTag(CAMERA_TAG)) 
        {
            OpenDoors();
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger zone
    /// Closes doors if the camera exits
    /// </summary>
    /// <param name="other">The collider that exited the trigger</param>
    private void OnTriggerExit(Collider other) 
    {
        // Check if the exiting collider is the main camera
        if (other.CompareTag(CAMERA_TAG)) 
        {
            CloseDoors();
        }
    }
}