using UnityEngine;
using UnityEngine.XR.Management;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Diagnostic utility for logging XR loader information
/// Helps debug XR initialization and loader configuration issues
/// </summary>
public class XRDiagnosticLogger : MonoBehaviour
{
    // Log prefix for easy filtering in console
    private const string LOG_PREFIX = "[XRDiagnosticLogger]";
    
    // Separator for loader names in output
    private const string LOADER_NAME_SEPARATOR = ", ";

    /// <summary>
    /// Initialize and log XR loader diagnostic information
    /// </summary>
    void Start()
    {
        LogRegisteredLoaders();
        LogActiveLoader();
    }

    /// <summary>
    /// Log all registered XR loaders in the system
    /// </summary>
    private void LogRegisteredLoaders()
    {
        // Get the XR manager instance
        XRManagerSettings xrManager = XRGeneralSettings.Instance.Manager;
        
        // Retrieve the list of registered loaders
        List<XRLoader> registeredLoaders = xrManager.loaders;
        
        // Extract loader names
        IEnumerable<string> loaderNames = registeredLoaders.Select(loader => loader.name);
        
        // Join names into a comma-separated string
        string loaderList = string.Join(LOADER_NAME_SEPARATOR, loaderNames);
        
        // Log the registered loaders
        Debug.Log(LOG_PREFIX + " Registered Loaders: " + loaderList);
    }

    /// <summary>
    /// Log the currently active XR loader if one exists
    /// </summary>
    private void LogActiveLoader()
    {
        // Get the XR manager instance
        XRManagerSettings xrManager = XRGeneralSettings.Instance.Manager;
        
        // Get the active loader reference
        XRLoader activeXRLoader = xrManager.activeLoader;
        
        // Check if there is an active loader
        if (activeXRLoader != null)
        {
            // Log the active loader name
            string activeLoaderName = activeXRLoader.name;
            Debug.Log(LOG_PREFIX + " Active Loader: " + activeLoaderName);
        }
        else
        {
            // Warn that no loader is currently active
            Debug.LogWarning(LOG_PREFIX + " No active loader currently active.");
        }
    }
}