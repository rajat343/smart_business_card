using UnityEngine;
using Vuforia;

/// <summary>
/// Controls visibility of AR tracked objects
/// Shows/hides children based on Vuforia tracking status
/// </summary>
[RequireComponent(typeof(ObserverBehaviour))]
public class TrackableVisibility : MonoBehaviour
{
    // Reference to the Vuforia observer component
    private ObserverBehaviour observer;

    /// <summary>
    /// Initialize observer and subscribe to tracking events
    /// </summary>
    void Start()
    {
        // Get the observer component
        observer = GetComponent<ObserverBehaviour>();
        
        // Subscribe to target status changes
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnStatusChanged;
        }

        // Start hidden until target is detected
        SetChildrenActive(false);
    }

    /// <summary>
    /// Clean up event subscriptions
    /// </summary>
    void OnDestroy()
    {
        // Unsubscribe from target status changes
        if (observer != null)
        {
            observer.OnTargetStatusChanged -= OnStatusChanged;
        }
    }

    /// <summary>
    /// Handle target tracking status changes
    /// </summary>
    /// <param name="behaviour">The observer behaviour</param>
    /// <param name="status">Current tracking status</param>
    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Determine if target is being tracked
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        
        // Update children visibility based on tracking status
        SetChildrenActive(isTracked);
    }

    /// <summary>
    /// Enable or disable child renderers, colliders, and canvases
    /// Skips any children with VideoPlaneController components
    /// </summary>
    /// <param name="active">Whether to activate or deactivate children</param>
    void SetChildrenActive(bool active)
    {
        // Enable/disable child Renderers
        // Skip any child that has a VideoPlaneController (video planes are controlled explicitly)
        foreach (var r in GetComponentsInChildren<Renderer>(true))
        {
            // Check if this renderer belongs to a video plane
            if (r.GetComponentInParent<VideoPlaneController>()) 
            {
                continue;
            }
            
            r.enabled = active;
        }
        
        // Enable/disable child Colliders
        foreach (var c in GetComponentsInChildren<Collider>(true))
        {
            // Check if this collider belongs to a video plane
            if (c.GetComponentInParent<VideoPlaneController>()) 
            {
                continue;
            }
            
            c.enabled = active;
        }
        
        // Enable/disable child Canvas elements
        foreach (var cv in GetComponentsInChildren<Canvas>(true))
        {
            // Check if this canvas belongs to a video plane
            if (cv.GetComponentInParent<VideoPlaneController>()) 
            {
                continue;
            }
            
            cv.enabled = active;
        }
    }
}