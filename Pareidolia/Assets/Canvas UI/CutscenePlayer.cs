using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutscenePlayer : MonoBehaviour
{

     private VideoPlayer video;
     public FadeExitScene FadeOutCanvas;

    void Start()
    {
        video = gameObject.GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();
    }


    private void OnMovieEnded(VideoPlayer vp)
    {
        FadeOutCanvas.FadeOutExit();
    }
}