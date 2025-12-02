using UnityEngine;
using UnityEngine.Video;
using Vuforia;

/// <summary>
/// Controls video playback on AR video planes
/// Manages video player, renderer, and audio components
/// Automatically stops video when tracking is lost
/// </summary>
[RequireComponent(typeof(VideoPlayer))]
public class VideoPlaneController : MonoBehaviour
{
    // Core component references
    private VideoPlayer vp;
    private Renderer rend;
    private AudioSource audioSource;
    private ObserverBehaviour parentObserver;

    /// <summary>
    /// Cache component references early
    /// </summary>
    void Awake()
    {
        // Get the video player component
        vp = GetComponent<VideoPlayer>();
        
        // Get the renderer component for visibility control
        rend = GetComponent<Renderer>();
        
        // Get the audio source component if present
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Initialize video settings and tracking events
    /// </summary>
    void Start()
    {
        // Hidden initially until explicitly played
        if (rend) 
        {
            rend.enabled = false;
        }
        
        // Don't autoplay on scene load
        if (vp) 
        {
            vp.playOnAwake = false;
        }

        // Subscribe to parent target tracking events
        parentObserver = GetComponentInParent<ObserverBehaviour>();
        if (parentObserver != null)
        {
            parentObserver.OnTargetStatusChanged += OnTargetStatusChanged;
        }
    }

    /// <summary>
    /// Clean up event subscriptions
    /// </summary>
    void OnDestroy()
    {
        // Unsubscribe from tracking events
        if (parentObserver != null)
        {
            parentObserver.OnTargetStatusChanged -= OnTargetStatusChanged;
        }
    }

    /// <summary>
    /// Handle parent target tracking status changes
    /// </summary>
    /// <param name="obs">The observer behaviour</param>
    /// <param name="status">Current tracking status</param>
    void OnTargetStatusChanged(ObserverBehaviour obs, TargetStatus status)
    {
        // Check if target is currently tracked
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        
        // Stop video automatically when target is lost
        if (!isTracked)
        {
            StopAndHide();
        }
    }

    /// <summary>
    /// Start playing video and show the video plane
    /// </summary>
    public void PlayAndShow()
    {
        // Make the video plane visible
        if (rend) 
        {
            rend.enabled = true;
        }

        // Configure audio output if audio source is available
        if (audioSource != null)
        {
            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.SetTargetAudioSource(0, audioSource);
        }

        // Start video playback
        vp.Play();
    }

    /// <summary>
    /// Stop video playback and hide the video plane
    /// </summary>
    public void StopAndHide()
    {
        // Stop the video player if it's playing
        if (vp != null && vp.isPlaying) 
        {
            vp.Stop();
        }
        
        // Stop the audio source
        if (audioSource != null) 
        {
            audioSource.Stop();
        }
        
        // Hide the video plane renderer
        if (rend != null) 
        {
            rend.enabled = false;
        }
    }

    /// <summary>
    /// Check if video is currently playing
    /// </summary>
    /// <returns>True if video is playing, false otherwise</returns>
    public bool IsPlaying()
    {
        return vp != null && vp.isPlaying;
    }
}