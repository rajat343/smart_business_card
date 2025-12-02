using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages button interactions on AR business cards
/// Handles raycast detection and action execution for AR buttons
/// </summary>
public class BusinessCardButtonManager : MonoBehaviour
{
    // Reference to the AR camera used for raycasting
    public Camera arCamera;
    
    // Maximum distance for raycast detection
    public float rayDistance = 100f;

    // Keeps track of the currently playing video
    private VideoPlaneController currentVideo = null;

    /// <summary>
    /// Initialize the manager and validate camera reference
    /// </summary>
    void Start()
    {
        // Try to find the main camera if not assigned
        if (arCamera == null) 
        {
            arCamera = Camera.main;
        }
        
        // Log error if still no camera found
        if (arCamera == null) 
        {
            Debug.LogError("[BCBM] No AR camera found. Tag your ARCamera as MainCamera or assign it in inspector.");
        }
    }

    /// <summary>
    /// Check for user input each frame
    /// </summary>
    void Update()
    {
        // Handle different input methods based on platform
#if UNITY_EDITOR
        // Mouse input for editor testing
        if (Input.GetMouseButtonDown(0)) 
        {
            HandleClick(Input.mousePosition);
        }
#else
        // Touch input for mobile devices
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) 
        {
            HandleClick(Input.touches[0].position);
        }
#endif
    }

    /// <summary>
    /// Process click/touch input and perform raycast
    /// </summary>
    /// <param name="screenPos">Screen position of the input</param>
    void HandleClick(Vector2 screenPos)
    {
        // Cast a ray from camera through the screen position
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Debug.Log("[BCBM] Raycast hit: " + hit.collider.gameObject.name);

            // Check if the hit object has an ARButton component
            ARButton btn = hit.collider.GetComponentInParent<ARButton>();
            if (btn == null) 
            { 
                Debug.Log("[BCBM] No ARButton on hit hierarchy."); 
                return; 
            }

            // Verify the button is on a tracked image target
            ObserverBehaviour obs = btn.GetComponentInParent<ObserverBehaviour>();
            if (obs == null) 
            { 
                Debug.Log("[BCBM] No ObserverBehaviour (ImageTarget) on parent. Ignoring."); 
                return; 
            }

            // Check tracking status
            var status = obs.TargetStatus;
            if (status.Status != Status.TRACKED && status.Status != Status.EXTENDED_TRACKED)
            {
                Debug.Log("[BCBM] Parent target not tracked (status: " + status.Status + "). Ignoring.");
                return;
            }

            // All checks passed, execute the button action
            ExecuteAction(btn);
        }
        else
        {
            Debug.Log("[BCBM] Raycast hit nothing");
        }
    }

    /// <summary>
    /// Execute the appropriate action based on button type
    /// </summary>
    /// <param name="btn">The ARButton that was clicked</param>
    void ExecuteAction(ARButton btn)
    {
        switch (btn.action)
        {
            case ButtonAction.Email:
                // Open default email client
                if (!string.IsNullOrEmpty(btn.actionData))
                {
                    Application.OpenURL("mailto:" + btn.actionData);
                }
                break;

            case ButtonAction.Phone:
                // Open phone dialer
                if (!string.IsNullOrEmpty(btn.actionData))
                {
                    Application.OpenURL("tel://" + btn.actionData);
                }
                break;

            case ButtonAction.Map:
                // Open maps URL
                if (!string.IsNullOrEmpty(btn.actionData))
                {
                    Application.OpenURL(btn.actionData);
                }
                break;

            case ButtonAction.Scene:
                // Load a different scene
                if (!string.IsNullOrEmpty(btn.actionData))
                {
                    SceneManager.LoadScene(btn.actionData);
                }
                break;

            case ButtonAction.Video:
                // Handle video playback toggle
                if (btn.targetObject == null) 
                { 
                    Debug.LogWarning("[BCBM] Video button has no targetObject assigned."); 
                    return; 
                }
                
                VideoPlaneController vpc = btn.targetObject.GetComponent<VideoPlaneController>();
                if (vpc == null) 
                { 
                    Debug.LogWarning("[BCBM] targetObject has no VideoPlaneController."); 
                    return; 
                }

                // Toggle video: if same video is playing, stop it
                if (currentVideo != null && currentVideo == vpc)
                {
                    currentVideo.StopAndHide();
                    currentVideo = null;
                }
                else
                {
                    // Stop any currently playing video
                    if (currentVideo != null) 
                    { 
                        currentVideo.StopAndHide(); 
                        currentVideo = null; 
                    }
                    
                    // Play the new video
                    vpc.PlayAndShow();
                    currentVideo = vpc;
                }
                break;
        }

        Debug.Log("[BCBM] Executed action: " + btn.action + " data:" + btn.actionData);
    }
}