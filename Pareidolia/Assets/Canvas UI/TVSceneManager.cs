using System;
using UnityEngine;
using UnityEngine.Video;

public class TVSceneManager : MonoBehaviour
{
    public static event Action TVWatchedEvent;
     private VideoPlayer video;

    void Start()
    {
        video = gameObject.GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();
    }


    private void OnMovieEnded(VideoPlayer vp)
    {
        TVWatchedEvent?.Invoke();
        SceneSwitcher.UnLoadSceneOnTop("TVWatch");
    }
}