using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;

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

    void Update() // let player skip the scene
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetMouseButtonDown(0))
        {
            OnMovieEnded(video);
        }
    }


    private void OnMovieEnded(VideoPlayer vp)
    {
        FadeOutCanvas.FadeOutExit();
    }
}