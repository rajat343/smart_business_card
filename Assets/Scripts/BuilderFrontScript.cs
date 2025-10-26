using UnityEngine;
using UnityEngine.Video;
using System.Collections;
#if VUFORIA_PRESENT
using Vuforia;
#endif

public class BuilderFrontScript : MonoBehaviour
{
    private Camera arCamera;
    public GameObject aboutPlane;
    public GameObject projectsPlane;

    private VideoPlayer currentVideo;

    void Start()
    {
        // ✅ Get AR camera safely for both Vuforia and non-Vuforia projects
        #if VUFORIA_PRESENT
        arCamera = VuforiaBehaviour.Instance?.transform.GetComponentInChildren<Camera>();
        #else
        arCamera = Camera.main;
        #endif

        if (arCamera == null)
        {
            Debug.LogError("[BuilderFrontScript] AR Camera not found!");
        }

        // ✅ Hide planes initially
        if (aboutPlane) aboutPlane.SetActive(false);
        if (projectsPlane) projectsPlane.SetActive(false);
    }

    void Update()
    {
        // ✅ Touch input for mobile
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
                HandleRaycast(touch.position);
        }

        // ✅ Mouse input for Unity Editor
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast(Input.mousePosition);
        }
    }

    private void HandleRaycast(Vector3 screenPosition)
    {
        if (arCamera == null)
        {
            Debug.LogWarning("[BuilderFrontScript] AR Camera missing!");
            return;
        }

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            string hitName = hit.collider.gameObject.name;
            Debug.Log("[BuilderFrontScript] Raycast hit: " + hitName);

            switch (hitName)
            {
                case "builder_email_btn":
                    Application.OpenURL("mailto:adam.smith@gmail.com");
                    break;

                case "builder_phone_btn":
                    Application.OpenURL("tel://4081111111");
                    break;

                case "builder_map_btn":
                    Application.OpenURL("https://www.google.co.in/maps/dir//southland+mall");
                    break;

                case "builder_about_btn":
                    ToggleVideo(aboutPlane);
                    break;

                case "builder_projects_btn":
                    ToggleVideo(projectsPlane);
                    break;

                // ✅ Allow closing by tapping the plane itself
                case "aboutPlane":
                case "projectsPlane":
                    CloseCurrentVideo();
                    break;

                default:
                    Debug.Log("[BuilderFrontScript] Hit object not a button");
                    break;
            }
        }
        else
        {
            Debug.Log("[BuilderFrontScript] Raycast hit nothing");
        }
    }

    private void ToggleVideo(GameObject plane)
    {
        if (plane == null) return;

        VideoPlayer vp = plane.GetComponent<VideoPlayer>();
        if (vp == null)
        {
            Debug.LogWarning("[BuilderFrontScript] No VideoPlayer found on " + plane.name);
            return;
        }

        // ✅ If plane already active → close it
        if (plane.activeSelf)
        {
            CloseCurrentVideo();
            return;
        }

        // ✅ Hide currently open plane if any
        if (currentVideo != null)
        {
            currentVideo.Stop();
            currentVideo.gameObject.SetActive(false);
            currentVideo = null;
        }

        // ✅ Show new plane after small delay (prevents collider timing issues)
        StartCoroutine(ActivateAndPlay(plane, vp));
    }

    private IEnumerator ActivateAndPlay(GameObject plane, VideoPlayer vp)
    {
        yield return null; // wait 1 frame for collider update
        plane.SetActive(true);
        vp.Play();
        currentVideo = vp;
        Debug.Log("[BuilderFrontScript] Playing video on " + plane.name);
    }

    private void CloseCurrentVideo()
    {
        if (currentVideo != null)
        {
            currentVideo.Stop();
            currentVideo.gameObject.SetActive(false);
            Debug.Log("[BuilderFrontScript] Closed video plane: " + currentVideo.gameObject.name);
            currentVideo = null;
        }
    }
}
