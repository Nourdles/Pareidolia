using UnityEngine;
using UnityEngine.Video;

public class TVSceneManager : MonoBehaviour
{

     private VideoPlayer video;

    void Start()
    {
        video = gameObject.GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();
    }


    private void OnMovieEnded(VideoPlayer vp)
    {
        SceneSwitcher.UnLoadSceneOnTop("TVWatch");
    }
}