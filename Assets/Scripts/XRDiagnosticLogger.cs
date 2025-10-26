using UnityEngine;
using UnityEngine.XR.Management;
using System.Linq;

public class XRDiagnosticLogger : MonoBehaviour
{
    void Start()
    {
        var loaders = XRGeneralSettings.Instance.Manager.loaders;
        Debug.Log("[XRDiagnosticLogger] Registered Loaders: " + string.Join(", ", loaders.Select(l => l.name)));

        if (XRGeneralSettings.Instance.Manager.activeLoader != null)
            Debug.Log("[XRDiagnosticLogger] Active Loader: " + XRGeneralSettings.Instance.Manager.activeLoader.name);
        else
            Debug.LogWarning("[XRDiagnosticLogger] No active loader currently active.");
    }
}
