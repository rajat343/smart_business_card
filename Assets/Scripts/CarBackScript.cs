using UnityEngine;
using UnityEngine.SceneManagement;

public class CarBackScript : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        // Ensure AR Camera has the "MainCamera" tag
        arCamera = Camera.main;
        Debug.Log("[CarBackScript] Initialized with AR Camera: " + (arCamera != null));
    }

    void Update()
    {
        // Touch input (on mobile)
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            HandleRaycast(Input.touches[0].position);
        }

        // Mouse click (for Unity Editor testing)
        if (Input.GetMouseButtonDown(0))
        {
            HandleRaycast(Input.mousePosition);
        }
    }

    private void HandleRaycast(Vector3 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("[CarBackScript] Raycast hit: " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.name == "car_back_btn")
            {
                Debug.Log("[CarBackScript] Back button pressed → loading scene: CarScene");
                SceneManager.LoadScene("CarScene"); // load by name
            }
            else
            {
                Debug.Log("[CarBackScript] Hit object is not the back button.");
            }
        }
        else
        {
            Debug.Log("[CarBackScript] Raycast hit nothing");
        }
    }
}
