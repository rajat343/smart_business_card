using UnityEngine;
using Vuforia;

[RequireComponent(typeof(ObserverBehaviour))]
public class TrackableVisibility : MonoBehaviour
{
    ObserverBehaviour observer;

    void Start()
    {
        observer = GetComponent<ObserverBehaviour>();
        if (observer != null)
            observer.OnTargetStatusChanged += OnStatusChanged;

        // start hidden until detected
        SetChildrenActive(false);
    }

    void OnDestroy()
    {
        if (observer != null)
            observer.OnTargetStatusChanged -= OnStatusChanged;
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        SetChildrenActive(isTracked);
    }

    void SetChildrenActive(bool active)
    {
        // enable/disable child Renderers/Colliders/Canvas,
        // but SKIP any child that has a VideoPlaneController (video planes are controlled explicitly)
        foreach (var r in GetComponentsInChildren<Renderer>(true))
        {
            if (r.GetComponentInParent<VideoPlaneController>()) continue;
            r.enabled = active;
        }
        foreach (var c in GetComponentsInChildren<Collider>(true))
        {
            if (c.GetComponentInParent<VideoPlaneController>()) continue;
            c.enabled = active;
        }
        foreach (var cv in GetComponentsInChildren<Canvas>(true))
        {
            if (cv.GetComponentInParent<VideoPlaneController>()) continue;
            cv.enabled = active;
        }
    }
}
