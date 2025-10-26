using UnityEngine;
using UnityEngine.Video;
using System.Collections;
#if VUFORIA_PRESENT
using Vuforia;
#endif

public class CarFrontScript : MonoBehaviour
{
    private Camera arCamera;

    public GameObject aboutPlane;
    public GameObject projectsPlane;

    private VideoPlayer currentVideo;

    void Start()
    {
        // ✅ Ensure correct ARCamera reference for both Vuforia and normal camera
        #if VUFORIA_PRESENT
        arCamera = VuforiaBehaviour.Instance?.transform.GetComponentInChildren<Camera>();
        #else
        arCamera = Camera.main;
        #endif

        if (arCamera == null)
        {
            Debug.LogError("[CarFrontScript] AR Camera not found!");
        }

        // ✅ Hide all video planes at start
        if (aboutPlane) aboutPlane.SetActive(false);
        if (projectsPlane) projectsPlane.SetActive(false);
    }

    void Update()
    {
        // ✅ Touch input (mobile)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Ended)
                HandleRaycast(touch.position);
        }

        // ✅ Mouse input (Editor)
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast(Input.mousePosition);
        }
    }

    private void HandleRaycast(Vector3 screenPosition)
    {
        if (arCamera == null) return;

        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Default")))
        {
            string hitName = hit.collider.gameObject.name;
            Debug.Log("[CarFrontScript] Raycast hit: " + hitName);

            switch (hitName)
            {
                case "car_email_btn":
                    Application.OpenURL("mailto:john.doe@gmail.com");
                    break;

                case "car_phone_btn":
                    Application.OpenURL("tel://5101111111");
                    break;

                case "car_map_btn":
                    Application.OpenURL("https://www.google.co.in/maps/dir//great+mall");
                    break;

                case "car_about_btn":
                    ToggleVideo(aboutPlane);
                    break;

                case "car_projects_btn":
                    ToggleVideo(projectsPlane);
                    break;

                // ✅ Allow tapping on plane itself to close
                case "aboutPlane":
                case "projectsPlane":
                    CloseCurrentVideo();
                    break;

                default:
                    Debug.Log("[CarFrontScript] Hit object not a button");
                    break;
            }
        }
    }

    private void ToggleVideo(GameObject plane)
    {
        if (plane == null) return;

        VideoPlayer vp = plane.GetComponent<VideoPlayer>();
        if (vp == null)
        {
            Debug.LogWarning("[CarFrontScript] No VideoPlayer found on " + plane.name);
            return;
        }

        // ✅ If plane already active → close it
        if (plane.activeSelf)
        {
            CloseCurrentVideo();
            return;
        }

        // ✅ Hide any currently open video plane
        if (currentVideo != null)
        {
            currentVideo.Stop();
            currentVideo.gameObject.SetActive(false);
            currentVideo = null;
        }

        // ✅ Small delay before activating to refresh colliders properly
        StartCoroutine(ActivateAndPlay(plane, vp));
    }

    private IEnumerator ActivateAndPlay(GameObject plane, VideoPlayer vp)
    {
        yield return null; // wait one frame
        plane.SetActive(true);
        vp.Play();
        currentVideo = vp;
        Debug.Log("[CarFrontScript] Playing video on " + plane.name);
    }

    private void CloseCurrentVideo()
    {
        if (currentVideo != null)
        {
            currentVideo.Stop();
            currentVideo.gameObject.SetActive(false);
            Debug.Log("[CarFrontScript] Closed video on " + currentVideo.gameObject.name);
            currentVideo = null;
        }
    }
}
