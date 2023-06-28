using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoClip HoldedVideo;
    public VideoManager videoManager;
    public bool IsExitButton = false;

    void Start()
    {
        if (!IsExitButton)
            videoManager = transform.GetComponentInParent<VideoManager>();
    }

    public void PlayVideo()
    {
        if (!IsExitButton)
            videoManager.PlayVideo(HoldedVideo);
        else
            videoManager.StopVideo();
    }
}