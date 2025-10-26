using UnityEngine;
using Vuforia;
using UnityEngine.SceneManagement;

public class BusinessCardButtonManager : MonoBehaviour
{
    public Camera arCamera;
    public float rayDistance = 100f;

    VideoPlaneController currentVideo = null;

    void Start()
    {
        if (arCamera == null) arCamera = Camera.main;
        if (arCamera == null) Debug.LogError("[BCBM] No AR camera found. Tag your ARCamera as MainCamera or assign it in inspector.");
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0)) HandleClick(Input.mousePosition);
#else
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) HandleClick(Input.touches[0].position);
#endif
    }

    void HandleClick(Vector2 screenPos)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Debug.Log("[BCBM] Raycast hit: " + hit.collider.gameObject.name);

            ARButton btn = hit.collider.GetComponentInParent<ARButton>();
            if (btn == null) { Debug.Log("[BCBM] No ARButton on hit hierarchy."); return; }

            ObserverBehaviour obs = btn.GetComponentInParent<ObserverBehaviour>();
            if (obs == null) { Debug.Log("[BCBM] No ObserverBehaviour (ImageTarget) on parent. Ignoring."); return; }

            var status = obs.TargetStatus;
            if (status.Status != Status.TRACKED && status.Status != Status.EXTENDED_TRACKED)
            {
                Debug.Log("[BCBM] Parent target not tracked (status: " + status.Status + "). Ignoring.");
                return;
            }

            ExecuteAction(btn);
        }
        else
        {
            Debug.Log("[BCBM] Raycast hit nothing");
        }
    }

    void ExecuteAction(ARButton btn)
    {
        switch (btn.action)
        {
            case ButtonAction.Email:
                if (!string.IsNullOrEmpty(btn.actionData))
                    Application.OpenURL("mailto:" + btn.actionData);
                break;

            case ButtonAction.Phone:
                if (!string.IsNullOrEmpty(btn.actionData))
                    Application.OpenURL("tel://" + btn.actionData);
                break;

            case ButtonAction.Map:
                if (!string.IsNullOrEmpty(btn.actionData))
                    Application.OpenURL(btn.actionData);
                break;

            case ButtonAction.Scene:
                if (!string.IsNullOrEmpty(btn.actionData))
                    SceneManager.LoadScene(btn.actionData);
                break;

            case ButtonAction.Video:
                if (btn.targetObject == null) { Debug.LogWarning("[BCBM] Video button has no targetObject assigned."); return; }
                VideoPlaneController vpc = btn.targetObject.GetComponent<VideoPlaneController>();
                if (vpc == null) { Debug.LogWarning("[BCBM] targetObject has no VideoPlaneController."); return; }

                if (currentVideo != null && currentVideo == vpc)
                {
                    currentVideo.StopAndHide();
                    currentVideo = null;
                }
                else
                {
                    if (currentVideo != null) { currentVideo.StopAndHide(); currentVideo = null; }
                    vpc.PlayAndShow();
                    currentVideo = vpc;
                }
                break;
        }

        Debug.Log("[BCBM] Executed action: " + btn.action + " data:" + btn.actionData);
    }
}
