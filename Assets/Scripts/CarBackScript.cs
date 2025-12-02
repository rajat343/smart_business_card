using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles back button interaction for the car scene
/// Uses raycast detection to load the previous scene
/// </summary>
public class CarBackScript : MonoBehaviour
{
    // Reference to the main AR camera
    private Camera arCamera;
    
    // Name of the button object to detect
    private const string BACK_BUTTON_NAME = "car_back_btn";
    
    // Name of the scene to load when back button is pressed
    private const string TARGET_SCENE_NAME = "CarScene";

    /// <summary>
    /// Initialize camera reference and log status
    /// </summary>
    void Start()
    {
        // Ensure AR Camera has the "MainCamera" tag
        arCamera = Camera.main;
        
        // Log initialization status for debugging
        Debug.Log("[CarBackScript] Initialized with AR Camera: " + (arCamera != null));
    }

    /// <summary>
    /// Check for user input each frame
    /// </summary>
    void Update()
    {
        // Handle touch input (on mobile devices)
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            HandleRaycast(Input.touches[0].position);
        }

        // Handle mouse click (for Unity Editor testing)
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast(Input.mousePosition);
        }
    }

    /// <summary>
    /// Perform raycast from screen position and check for back button hit
    /// </summary>
    /// <param name="screenPosition">Screen position of the input</param>
    private void HandleRaycast(Vector3 screenPosition)
    {
        // Create ray from camera through screen position
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        // Cast ray and check for collision
        if (Physics.Raycast(ray, out hit))
        {
            // Log what object was hit
            Debug.Log("[CarBackScript] Raycast hit: " + hit.collider.gameObject.name);

            // Check if the hit object is the back button
            if (hit.collider.gameObject.name == BACK_BUTTON_NAME)
            {
                // Log scene transition
                Debug.Log("[CarBackScript] Back button pressed → loading scene: " + TARGET_SCENE_NAME);
                
                // Load the target scene by name
                SceneManager.LoadScene(TARGET_SCENE_NAME);
            }
            else
            {
                // Log that a different object was hit
                Debug.Log("[CarBackScript] Hit object is not the back button.");
            }
        }
        else
        {
            // Log that raycast didn't hit anything
            Debug.Log("[CarBackScript] Raycast hit nothing");
        }
    }
}