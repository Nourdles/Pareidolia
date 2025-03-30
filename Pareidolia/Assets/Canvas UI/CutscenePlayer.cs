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
     public Canvas promptCanvas;
     private bool promptShown = false;
     private bool canSkip = false;

    void Start()
    {
        video = gameObject.GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();

        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(false); // hide at start
    }

    void Update() // let player skip the scene
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }

    void HandleClick()
    {
        if (!promptShown)
        {
            promptShown = true;

            if (promptCanvas != null)
                promptCanvas.gameObject.SetActive(true);

            // start 2s cooldown before next click can trigger fade
            StartCoroutine(CooldownBeforeFade());
        }
        else if (canSkip)
        {
            OnMovieEnded(video);
        }
    }

    IEnumerator CooldownBeforeFade()
    {
        yield return new WaitForSeconds(2f);
        canSkip = true;
    }

    private void OnMovieEnded(VideoPlayer vp)
    {
        FadeOutCanvas.FadeOutExit();
    }
}