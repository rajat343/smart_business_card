using UnityEngine;
using Vuforia;

/// <summary>
/// Enables or disables colliders on child objects based on tracking status
/// Automatically toggles colliders when Vuforia target is tracked or lost
/// </summary>
public class EnableCollidersOnTrack : MonoBehaviour
{
    // Reference to the Vuforia observer behaviour component
    private ObserverBehaviour targetObserver;
    
    // Cache for child colliders to avoid repeated GetComponentsInChildren calls
    private Collider[] cachedColliders;

    /// <summary>
    /// Initialize observer and cache colliders
    /// </summary>
    void Awake()
    {
        // Get the observer component attached to this GameObject
        targetObserver = GetComponent<ObserverBehaviour>();
        
        if (targetObserver != null)
        {
            // Subscribe to tracking status changes
            targetObserver.OnTargetStatusChanged += OnTargetStatusChanged;
        }
        
        // Cache all child colliders for better performance
        cachedColliders = GetComponentsInChildren<Collider>(true);
    }

    /// <summary>
    /// Clean up event subscriptions on destroy
    /// </summary>
    private void OnDestroy()
    {
        if (targetObserver != null)
        {
            // Unsubscribe from tracking events to prevent memory leaks
            targetObserver.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    /// <summary>
    /// Handle changes in target tracking status
    /// </summary>
    /// <param name="behaviour">The observer behaviour that triggered the event</param>
    /// <param name="status">The current tracking status</param>
    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        // Determine if the target is currently being tracked
        bool shouldEnableColliders = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        // Toggle all child colliders based on tracking status
        int colliderCount = cachedColliders.Length;
        for (int i = 0; i < colliderCount; i++)
        {
            if (cachedColliders[i] != null)
            {
                cachedColliders[i].enabled = shouldEnableColliders;
            }
        }

        // Log the state change for debugging purposes
        string stateMessage = shouldEnableColliders ? "enabled" : "disabled";
        Debug.Log("[EnableCollidersOnTrack] Colliders " + stateMessage + " for " + gameObject.name);
    }
}