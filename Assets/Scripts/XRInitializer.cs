using UnityEngine;
using System.Collections;
using UnityEngine.XR.Management;

public class XRInitializer : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("[XRInitializer] Begin manual XR initialization...");
        var xrManager = XRGeneralSettings.Instance.Manager;

        if (xrManager == null)
        {
            Debug.LogError("[XRInitializer] XR Manager not found.");
            yield break;
        }

        // Initialize loader synchronously (or InitializeLoader() coroutine)
        yield return xrManager.InitializeLoader();

        if (xrManager.activeLoader == null)
        {
            Debug.LogError("[XRInitializer] Failed to initialize XR loader.");
            yield break;
        }

        // Start XR subsystems (display/input)
        xrManager.StartSubsystems();
        Debug.Log("[XRInitializer] XR subsystems started.");
    }
}
