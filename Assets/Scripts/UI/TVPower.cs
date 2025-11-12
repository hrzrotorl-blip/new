using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVPower : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Stop();
        }
    }

    public void TurnOn()
    {
        if (videoPlayer != null)
        {
            if (!videoPlayer.isPrepared)
            {
                videoPlayer.Prepare();
                videoPlayer.prepareCompleted += OnPreparedAndPlay;
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }

    public void TurnOff()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
        }
    }

    void OnPreparedAndPlay(VideoPlayer vp)
    {
        vp.prepareCompleted -= OnPreparedAndPlay;
        vp.Play();
    }
}

