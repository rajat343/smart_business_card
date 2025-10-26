// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Vuforia;

// public class BuilderBackScript : MonoBehaviour
// {
//     private Camera arCamera;

//     void Start()
//     {
//         arCamera = Camera.main;
//     }

//     void Update()
//     {
//         if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
//             HandleRaycast(Input.touches[0].position);

//         if (Input.GetMouseButtonDown(0))
//             HandleRaycast(Input.mousePosition);
//     }

//     private void HandleRaycast(Vector3 screenPosition)
//     {
//         Ray ray = arCamera.ScreenPointToRay(screenPosition);
//         RaycastHit hit;

//         if (Physics.Raycast(ray, out hit))
//         {
//             if (hit.collider.gameObject.name == "builder_back_btn")
//             {
//                 Debug.Log("[BuilderBackScript] Stopping Vuforia and loading VR scene...");
//                 VuforiaBehaviour.Instance.enabled = false;  // disable AR system before switching
//                 SceneManager.LoadScene("BuilderScene");
//             }
//         }
//     }
// }






// using UnityEngine;
// using UnityEngine.SceneManagement;
// using Vuforia;
// using System.Collections;

// public class BuilderBackScript : MonoBehaviour
// {
//     private Camera arCamera;

//     void Start()
//     {
//         arCamera = Camera.main;
//         if (arCamera == null)
//             Debug.LogError("[BuilderBackScript] No main camera found!");
//     }

//     void Update()
//     {
// #if UNITY_EDITOR
//         if (Input.GetMouseButtonDown(0))
//             HandleRaycast(Input.mousePosition);
// #else
//         if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
//             HandleRaycast(Input.touches[0].position);
// #endif
//     }

//     private void HandleRaycast(Vector3 screenPosition)
//     {
//         Ray ray = arCamera.ScreenPointToRay(screenPosition);
//         if (Physics.Raycast(ray, out RaycastHit hit))
//         {
//             if (hit.collider != null && hit.collider.gameObject.name == "builder_back_btn")
//             {
//                 Debug.Log("[BuilderBackScript] builder_back_btn tapped. Switching to VR...");
//                 StartCoroutine(SwitchToVRScene());
//             }
//         }
//     }

//     private IEnumerator SwitchToVRScene()
//     {
//         Debug.Log("[BuilderBackScript] Disabling Vuforia before switching...");

//         // ✅ Safely disable Vuforia
//         if (VuforiaBehaviour.Instance != null)
//         {
//             VuforiaBehaviour.Instance.enabled = false;
//             Debug.Log("[BuilderBackScript] VuforiaBehaviour disabled.");
//         }

//         // ✅ Optionally disable AR camera (to fully stop rendering)
//         if (arCamera != null)
//         {
//             arCamera.enabled = false;
//             Debug.Log("[BuilderBackScript] AR Camera disabled.");
//         }

//         // Short delay to ensure clean shutdown
//         yield return new WaitForSeconds(0.5f);

//         Debug.Log("[BuilderBackScript] Loading BuilderScene...");
//         SceneManager.LoadScene("BuilderScene");
//     }
// }





using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;
using System.Collections;

public class BuilderBackScript : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        arCamera = Camera.main;
        if (arCamera == null)
            Debug.LogError("[BuilderBackScript] No main camera found!");
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
            HandleRaycast(Input.mousePosition);
#else
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
            HandleRaycast(Input.touches[0].position);
#endif
    }

    private void HandleRaycast(Vector3 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.gameObject.name == "builder_back_btn")
            {
                Debug.Log("[BuilderBackScript] builder_back_btn tapped. Switching to VR...");
                StartCoroutine(SwitchToVRScene());
            }
        }
    }

    private IEnumerator SwitchToVRScene()
    {
        Debug.Log("[BuilderBackScript] Stopping Vuforia before switching...");

        // ✅ Safely disable and deinitialize Vuforia
        if (VuforiaBehaviour.Instance != null)
        {
            VuforiaBehaviour.Instance.enabled = false;
            Debug.Log("[BuilderBackScript] VuforiaBehaviour disabled.");
        }

        // ✅ Deinitialize Vuforia completely
        if (VuforiaApplication.Instance != null)
        {
            VuforiaApplication.Instance.Deinit();
            Debug.Log("[BuilderBackScript] VuforiaApplication deinitialized.");
        }

        // ✅ Disable AR camera rendering (optional)
        if (arCamera != null)
        {
            arCamera.enabled = false;
            Debug.Log("[BuilderBackScript] AR Camera disabled.");
        }

        // Small delay to allow shutdown to finish
        yield return new WaitForSeconds(0.5f);
        Debug.Log("[BuilderBackScript] Loading VR scene...");
        SceneManager.LoadScene("BuilderScene");
    }
}
