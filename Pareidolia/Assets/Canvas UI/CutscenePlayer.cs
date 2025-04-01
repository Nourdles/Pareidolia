using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    private VideoPlayer video;
    public FadeExitScene FadeOutCanvas;

    public Canvas promptCanvas;
    public Image progressFill;
    public Image progressBase;
    public GameObject progressHandle;

    private float fillAmount = 0f;
    private float fillSpeed = 0.4f; // how quickly bar fills when held
    private float drainSpeed = 0.5f; // how quickly bar drains when released

    private bool promptVisible = false;
    private bool holdingSkip = false;
    private float lastInteractionTime = 0f;
    private float inactivityThreshold = 5f; // num seconds before hiding prompt
    private bool hasSkipped = false;

    private void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();

        SetPromptVisible(false);
    }

    private void Update()
    {
        bool keyboardHeld = Keyboard.current != null && Keyboard.current.eKey.isPressed;
        bool gamepadHeld = Gamepad.current != null && Gamepad.current.buttonSouth.isPressed; // A button

        bool clickPressed = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool keyboardPressed = Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
        bool gamepadPressed = Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame;

        // if any input happens (click or hold), show the prompt
        if (!promptVisible && (clickPressed || keyboardPressed || gamepadPressed))
        {
            SetPromptVisible(true);
            lastInteractionTime = Time.unscaledTime;
        }

        if (promptVisible)
        {
            // check if skip key is being held
            holdingSkip = keyboardHeld || gamepadHeld;

            if (holdingSkip)
            {
                fillAmount += fillSpeed * Time.unscaledDeltaTime;
                lastInteractionTime = Time.unscaledTime;
            }
            else if (fillAmount < 1f)
            {
                fillAmount -= drainSpeed * Time.unscaledDeltaTime;
            }

            fillAmount = Mathf.Clamp01(fillAmount);
            progressFill.fillAmount = fillAmount;

            if (fillAmount >= 1f)
            {
                SkipCutscene();
            }

            // hide after inactivity
            if (Time.unscaledTime - lastInteractionTime > inactivityThreshold)
            {
                SetPromptVisible(false);
                fillAmount = 0f;
            }
        }
    }

    private void SetPromptVisible(bool visible)
    {
        promptVisible = visible;

        if (promptCanvas != null)
            promptCanvas.gameObject.SetActive(visible);

        if (progressBase != null)
            progressBase.enabled = visible;

        if (progressFill != null)
            progressFill.enabled = visible;

        if (progressHandle != null)
        {
            Image handleImg = progressHandle.GetComponent<Image>();
            if (handleImg != null)
                handleImg.enabled = visible;
        }
    }

    private void OnMovieEnded(VideoPlayer vp)
    {
        if (hasSkipped) return;
        hasSkipped = true;
        FadeOutCanvas.FadeOutExit();
    }

    private void SkipCutscene() // skip it manually
    {
        if (hasSkipped) return;
        hasSkipped = true;
        FadeOutCanvas.FadeOutExit();
    }
}
