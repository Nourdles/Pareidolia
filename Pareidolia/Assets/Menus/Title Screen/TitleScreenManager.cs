using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using FMODUnity;

/// <summary>
/// Script for playing intro video, then switching to title screen video and making title screen buttons fade in.
/// </summary>

public class TitleScreenManager : MonoBehaviour
{
    public VideoPlayer firstVideoPlayer; // VideoPlayer for the first video
    public VideoPlayer secondVideoPlayer; // VideoPlayer for the second video
    public VideoClip fadeInVideo;
    public VideoClip titleScreenVideo;
    public CanvasGroup buttonGroup;
    public EventReference titleScreenMusic; 

    private bool buttonsFaded = false;
    private FMOD.Studio.EventInstance titleScreenMusicInstance;

    void Start()
    {
        buttonGroup.alpha = 0;
        firstVideoPlayer.targetCameraAlpha = 1.0f;
        secondVideoPlayer.targetCameraAlpha = 1.0f;

        StartCoroutine(PlayVideos());
    }

    IEnumerator PlayVideos()
    {
        firstVideoPlayer.clip = fadeInVideo;
        firstVideoPlayer.Prepare();

        while (!firstVideoPlayer.isPrepared)
            yield return null;

        firstVideoPlayer.Play();

        titleScreenMusicInstance = FMODUnity.RuntimeManager.CreateInstance(titleScreenMusic);
        titleScreenMusicInstance.start();  // Added this to play music on title screen

        secondVideoPlayer.clip = titleScreenVideo;
        secondVideoPlayer.Prepare();

        yield return new WaitForSeconds((float)fadeInVideo.length - 0.5f);

        while (!secondVideoPlayer.isPrepared)
            yield return null;

        secondVideoPlayer.Play();
        secondVideoPlayer.isLooping = true;
        firstVideoPlayer.Stop();

        if (!buttonsFaded)
        {
            buttonsFaded = true;
            StartCoroutine(FadeInButtons());
        }
    }

    IEnumerator FadeInButtons()
    {
        float duration = 1.5f;
        float time = 0;

        while (time < duration)
        {
            buttonGroup.alpha = Mathf.Lerp(0, 1, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        buttonGroup.alpha = 1;
    }
    private void OnDestroy() // This should stop the music when starting game
    {
        if (titleScreenMusicInstance.isValid())
        {
            titleScreenMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            titleScreenMusicInstance.release();
        }
    }
}
