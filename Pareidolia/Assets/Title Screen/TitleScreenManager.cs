using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;

public class TitleScreenManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;   // Assign VideoPlayer in Inspector
    public VideoClip fadeInVideo;     // First video (fade-in)
    public VideoClip titleScreenVideo; // Second video (looping title screen)
    public CanvasGroup buttonGroup;   // To handle button fade-in

    private bool buttonsFaded = false; // Ensure buttons fade in only once

    void Start()
    {
        buttonGroup.alpha = 0;  // Ensure buttons are invisible at the start
        StartCoroutine(PlayFirstVideo());
    }

    IEnumerator PlayFirstVideo()
    {
        videoPlayer.clip = fadeInVideo;
        videoPlayer.Prepare();

        // Wait until the first video is fully prepared
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        StartCoroutine(PreloadSecondVideo()); // Start preparing the next video early
    }

    IEnumerator PreloadSecondVideo()
    {
        yield return new WaitForSeconds((float)fadeInVideo.length - 1f); // Load just before first video ends

        videoPlayer.clip = titleScreenVideo;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null; // Ensure second video is fully loaded before switching

        videoPlayer.Play(); // Switch instantly
        videoPlayer.isLooping = true;

        if (!buttonsFaded)
        {
            buttonsFaded = true; // Ensure it only fades once
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

        buttonGroup.alpha = 1; // Ensure full opacity
    }
}
