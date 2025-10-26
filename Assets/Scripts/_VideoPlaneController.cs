using UnityEngine;
using UnityEngine.Video;
using Vuforia;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlaneController : MonoBehaviour
{
    VideoPlayer vp;
    Renderer rend;
    AudioSource audioSource;
    ObserverBehaviour parentObserver;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();
        rend = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        if (rend) rend.enabled = false;         // hidden initially
        if (vp) vp.playOnAwake = false;         // don't autoplay

        parentObserver = GetComponentInParent<ObserverBehaviour>();
        if (parentObserver != null)
            parentObserver.OnTargetStatusChanged += OnTargetStatusChanged;
    }

    void OnDestroy()
    {
        if (parentObserver != null)
            parentObserver.OnTargetStatusChanged -= OnTargetStatusChanged;
    }

    void OnTargetStatusChanged(ObserverBehaviour obs, TargetStatus status)
    {
        bool isTracked = status.Status == Status.TRACKED || status.Status == Status.EXTENDED_TRACKED;
        if (!isTracked)
            StopAndHide();   // stop video automatically when target lost
    }

    public void PlayAndShow()
    {
        if (rend) rend.enabled = true;

        if (audioSource != null)
        {
            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.SetTargetAudioSource(0, audioSource);
        }

        vp.Play();
    }

    public void StopAndHide()
    {
        if (vp != null && vp.isPlaying) vp.Stop();
        if (audioSource != null) audioSource.Stop();
        if (rend != null) rend.enabled = false;
    }

    public bool IsPlaying()
    {
        return vp != null && vp.isPlaying;
    }
}
