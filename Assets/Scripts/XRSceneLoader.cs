using System.Collections;
using UnityEngine;
using UnityEngine.XR.Management;

public class XRSceneLoader : MonoBehaviour
{
    IEnumerator Start()
    {
        Debug.Log("[XRSceneLoader] Starting XR initialization for Cardboard...");

        // Small delay to let AR unload completely
        yield return new WaitForSeconds(0.5f);

        // Initialize XR loader
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("[XRSceneLoader] ❌ Failed to initialize XR loader!");
        }
        else
        {
            XRGeneralSettings.Instance.Manager.StartSubsystems();
            Debug.Log("[XRSceneLoader] ✅ XR started successfully (Cardboard).");
        }
    }

    private void OnDestroy()
    {
        Debug.Log("[XRSceneLoader] Stopping XR before scene unload...");
        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
    }
}
