using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using FMODUnity;

public class jvnTitleScreenManager : MonoBehaviour
{
    // assigning the UI elements in the inspector
    public VideoPlayer videoPlayer;                 // video player
    public VideoClip fadeInVideo;                   // first video (fade-in)
    public VideoClip titleScreenVideo;              // second video (looping title screen)
    public CanvasGroup buttonGroup;                 // button fade-in
    public GameObject mainMenu;                     // container for main menu objects
    public GameObject optionsMenu;                  // container for options menu objects
    public GameObject creditsMenu;                  // container for credits menu objects
    public Button backButton;                       // back button
    public EventReference titleScreenMusic;         // unity event for music
    private FMOD.Studio.EventInstance titleScreenMusicInstance;
    private FMOD.Studio.EventInstance optionsMenuEventInstance;

    private bool buttonsFaded = false;              // ensure buttons fade in only once
    private bool videoSkipped = false;              // track if the video has been skipped
    private string currentMenu = "main";            // track the current menu state

    void Start()
    {
        buttonGroup.alpha = 0;                      // buttons are invisible at the start
        mainMenu.SetActive(true);                   // show main menu at the start
        optionsMenu.SetActive(false);               // hide options menu at the start
        creditsMenu.SetActive(false);               // hide credits menu at the start
        backButton.gameObject.SetActive(false);     // hide Back button at the start
        StartCoroutine(PlayFirstVideo());

        optionsMenuEventInstance = FMODUnity.RuntimeManager.CreateInstance("event:/Music/Title Screen Music"); // Replace with your actual event path
    }

    // skip the intro video if the player clicks the mouse
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !videoSkipped)
        {
            SkipIntro();
        }
    }

    IEnumerator PlayFirstVideo()
    {
        videoPlayer.clip = fadeInVideo;
        videoPlayer.Prepare();

        // wait until the first video is fully prepared
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        StartCoroutine(PreloadSecondVideo());       // start preparing the next video early
        titleScreenMusicInstance = FMODUnity.RuntimeManager.CreateInstance(titleScreenMusic);
        titleScreenMusicInstance.start();           // start title screen music
    }

    IEnumerator PreloadSecondVideo()
    {
        // wait until the first video finishes playing
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        videoPlayer.clip = titleScreenVideo;
        videoPlayer.Prepare();

        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        videoPlayer.isLooping = true;

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

    void SkipIntro()
    {
        videoSkipped = true;
        StopAllCoroutines();
        videoPlayer.Stop();
        videoPlayer.clip = titleScreenVideo;
        videoPlayer.Prepare();

    
        StartCoroutine(WaitForVideoPrepared());
    }

    IEnumerator WaitForVideoPrepared()
    {
        while (!videoPlayer.isPrepared)
            yield return null;

        videoPlayer.Play();
        videoPlayer.isLooping = true;

        if (!buttonsFaded)
        {
            buttonsFaded = true;
            StartCoroutine(FadeInButtons());
        }
    }

    public void ShowOptionsMenu()
    {
        currentMenu = "options";
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        backButton.gameObject.SetActive(true);

        optionsMenuEventInstance.setParameterByName("Options Menu", 1.0f);
    }

    public void ShowCreditsMenu()
    {
        currentMenu = "credits";
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        backButton.gameObject.SetActive(true);      // show Back button
    }

    public void HideCurrentMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        backButton.gameObject.SetActive(false);     // hide Back button

        currentMenu = "main";                       // reset to main menu state

        optionsMenuEventInstance.setParameterByName("Options Menu", 0.0f);
    }
    private void OnDestroy() // This should stop the music when starting game
    {
        if (titleScreenMusicInstance.isValid())
        {
            titleScreenMusicInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            titleScreenMusicInstance.release();
        }

        if (optionsMenuEventInstance.isValid())
        {
            optionsMenuEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            optionsMenuEventInstance.release();
        }
    }
}