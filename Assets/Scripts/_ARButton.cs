using UnityEngine;

// Button action types for AR interactions
public enum ButtonAction 
{ 
    Email, 
    Phone, 
    Map, 
    Scene, 
    Video 
}

/// <summary>
/// ARButton component for handling various AR button interactions
/// </summary>
public class ARButton : MonoBehaviour
{
    // The type of action this button will perform
    public ButtonAction action = ButtonAction.Email;
    
    [Tooltip("email / phone / url / sceneName depending on action")]
    public string actionData;
    
    [Tooltip("Only for Video action: assign the video plane GameObject here")]
    public GameObject targetObject;

    // Placeholder method for future initialization logic
    private void Start()
    {
        // TODO: Add initialization code here if needed
    }

    // Placeholder method for future update logic
    private void Update()
    {
        // TODO: Add per-frame logic here if needed
    }
}