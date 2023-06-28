using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject menu;
    public GameObject[] videoHolders;

    private void Start()
    {
        videoPlayer.gameObject.SetActive(false);
        videoPlayer.loopPointReached += StopVideo;
    }

    public void PlayVideo(VideoClip video)
    {
        videoPlayer.gameObject.SetActive(true);

        videoPlayer.clip = video;
        WorkWithHolders(false);
        videoPlayer.Play();
    }

    public void StopVideo(UnityEngine.Video.VideoPlayer vp)
    {
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        WorkWithHolders(true);
    }
    
    public void StopVideo()
    {
        videoPlayer.Stop();
        videoPlayer.gameObject.SetActive(false);
        WorkWithHolders(true);
    }

    void WorkWithHolders(bool Position)
    {
        foreach(GameObject video in videoHolders) {
            video.SetActive(Position);
        } 
        menu.SetActive(Position);
    }
}
