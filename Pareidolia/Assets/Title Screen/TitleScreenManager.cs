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
    public GameObject mainMenu;       // Container for main menu objects
    public GameObject optionsMenu;    // Container for options menu objects
    public GameObject creditsMenu;    // Container for credits menu objects
    public Button backButton;         // Reference to the Back button

    private bool buttonsFaded = false; // Ensure buttons fade in only once
    private bool videoSkipped = false; // Track if the video has been skipped
    private string currentMenu = "main"; // Track the current menu state

    void Start()
    {
        buttonGroup.alpha = 0;  // Ensure buttons are invisible at the start
        mainMenu.SetActive(true); // Show main menu at the start
        optionsMenu.SetActive(false); // Hide options menu at the start
        creditsMenu.SetActive(false); // Hide credits menu at the start
        backButton.gameObject.SetActive(false); // Hide Back button at the start
        StartCoroutine(PlayFirstVideo());
    }

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

    void SkipIntro()
    {
        videoSkipped = true;
        StopAllCoroutines(); // Stop any ongoing coroutines
        videoPlayer.Stop(); // Stop the current video
        videoPlayer.clip = titleScreenVideo;
        videoPlayer.Prepare();

        // Wait until the second video is fully prepared
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
            buttonsFaded = true; // Ensure it only fades once
            StartCoroutine(FadeInButtons());
        }
    }

    public void ShowOptionsMenu()
    {
        currentMenu = "options";
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        backButton.gameObject.SetActive(true); // Show Back button
    }

    public void ShowCreditsMenu()
    {
        currentMenu = "credits";
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        backButton.gameObject.SetActive(true); // Show Back button
    }

    public void HideCurrentMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        backButton.gameObject.SetActive(false); // Hide Back button

        currentMenu = "main"; // Reset to main menu state
    }
}