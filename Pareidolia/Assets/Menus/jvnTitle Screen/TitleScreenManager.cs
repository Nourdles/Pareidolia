using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class TitleScreenManager : MonoBehaviour
{
    public VideoPlayer introVideo;
    public VideoPlayer loopingVideo;
    public MenuManager menuManager;
    public MainMenuFader menuFader;
    public GameObject mainMenuUI;
    private bool hasFadedIn = false; // prevents fade from triggering multiple times
    private bool hasSkipped = false; // prevent ShowMainMenu being triggered whenever the main menu screen is clicked


    void Start()
    {
        // hide the Main Menu UI at start
        mainMenuUI.SetActive(false);

        // hide the looping vid at start
        loopingVideo.gameObject.SetActive(false);

        // start the intro vid
        introVideo.Play();
        introVideo.loopPointReached += OnIntroFinished; // event for when vid ends
    }

    void Update()
    {
        // skip input uses "B" (xbox) / "circle" (playstation) OR left click
        if (!hasSkipped && Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetMouseButtonDown(0))
        {
            SkipIntro();
        }
    }

    void SkipIntro()
    {
        if (hasSkipped) return; // already skipped
        
        // stop intro vid and play looping vid
        hasSkipped = true;
        introVideo.Stop();
        StartLoopingVideo();
    }

    void OnIntroFinished(VideoPlayer vp)
    {
        if (hasSkipped) return;
        hasSkipped = true;

        StartLoopingVideo();
    }

    void StartLoopingVideo()
    {
        // hide intro vid GameObject
        introVideo.gameObject.SetActive(false);

        // activate and start looping vid
        loopingVideo.gameObject.SetActive(true);
        loopingVideo.Play();

        // show main menu, but fade in the buttons only once
        mainMenuUI.SetActive(true);
        menuManager.ShowMainMenu();

        if (!hasFadedIn)
        {
            menuFader.StartFadeIn(); // trigger fade-in
            hasFadedIn = true; // prevent fade-in from happening again (this was an annoying issue where using options or credits, then going back would cause the buttons to fade in again)
        }
    }
}
