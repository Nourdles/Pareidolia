using System;
using UnityEngine;
using UnityEngine.Video;

public class TVSceneManager : MonoBehaviour
{
    public static event Action TVWatchedEvent;

    private VideoPlayer video;
    private GameObject overlayQuad;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;

        PauseManager.PauseGameEvent += OnPauseStateChanged;

        video.Play();
    }

    private void OnPauseStateChanged(bool isPaused)
    {
        if (video == null) return;

        if (isPaused)
        {
            video.Pause();
        }
        else
        {
            video.Play();
        }
    }

    private void OnMovieEnded(VideoPlayer vp)
    {
        TVWatchedEvent?.Invoke();
        SceneSwitcher.UnLoadSceneOnTop("TVWatch");
    }

    private void OnDestroy()
    {
        PauseManager.PauseGameEvent -= OnPauseStateChanged;
    }
}
