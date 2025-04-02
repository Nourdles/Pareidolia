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
        video.Play();
    }

    private void OnMovieEnded(VideoPlayer vp)
    {
        TVWatchedEvent?.Invoke();
        SceneSwitcher.UnLoadSceneOnTop("TVWatch");
    }
}
