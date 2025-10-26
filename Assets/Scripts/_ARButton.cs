using UnityEngine;

public enum ButtonAction { Email, Phone, Map, Scene, Video }

public class ARButton : MonoBehaviour
{
    public ButtonAction action = ButtonAction.Email;
    [Tooltip("email / phone / url / sceneName depending on action")]
    public string actionData;
    [Tooltip("Only for Video action: assign the video plane GameObject here")]
    public GameObject targetObject;
}
