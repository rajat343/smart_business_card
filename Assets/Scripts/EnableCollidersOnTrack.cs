using UnityEngine;
using Vuforia;

public class EnableCollidersOnTrack : MonoBehaviour
{
    private ObserverBehaviour observer;

    void Awake()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
        {
            observer.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    private void OnDestroy()
    {
        if (observer != null)
        {
            observer.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    private void OnTargetStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;

        foreach (var collider in GetComponentsInChildren<Collider>(true))
        {
            collider.enabled = isTracked;
        }

        Debug.Log("[EnableCollidersOnTrack] Colliders " + (isTracked ? "enabled" : "disabled") + " for " + gameObject.name);
    }
}
