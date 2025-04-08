using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;

public class CutscenePlayer : MonoBehaviour
{
    private VideoPlayer video;
    public FadeExitScene FadeOutCanvas;

    public Canvas promptCanvas;
    public Image progressFill;
    public Image progressBase;
    public GameObject progressHandle;

    private CanvasGroup promptGroup;
    private Coroutine fadeCoroutine;

    private float fadeDuration = 0.5f;
    private float fillAmount = 0f;
    private float fillSpeed = 0.4f;
    private float drainSpeed = 0.5f;

    private bool promptVisible = false;
    private bool holdingSkip = false;
    private float lastInteractionTime = 0f;
    private float inactivityThreshold = 5f;
    private bool hasSkipped = false;

    private void Start()
    {
        video = GetComponent<VideoPlayer>();
        video.loopPointReached += OnMovieEnded;
        video.Play();

        promptGroup = promptCanvas.GetComponent<CanvasGroup>();
        if (promptGroup == null)
            promptGroup = promptCanvas.gameObject.AddComponent<CanvasGroup>();

        promptGroup.alpha = 0f;
        promptGroup.interactable = false;
        promptGroup.blocksRaycasts = false;

        SetPromptVisible(false);
    }

    private void Update()
    {
        // detect keyboard key
        bool anyKeyboardPressed = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;

        // detect gamepad button
        bool anyGamepadPressed = Gamepad.current != null &&
            Gamepad.current.allControls.Any(control => control is ButtonControl button && button.wasPressedThisFrame);

        // detect mouse click (left or right)
        bool clickPressed = Mouse.current != null &&
            (Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame);

        // combined input check
        bool anyInputPressed = anyKeyboardPressed || anyGamepadPressed || clickPressed;

        if (!promptVisible && anyInputPressed)
        {
            SetPromptVisible(true);
            lastInteractionTime = Time.unscaledTime;
        }

        if (promptVisible)
        {
            holdingSkip =
                (Keyboard.current != null && Keyboard.current.anyKey.isPressed) ||
                (Gamepad.current != null && Gamepad.current.allControls.Any(control => control is ButtonControl button && button.isPressed)) ||
                (Mouse.current != null && (
                    Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed
                ));

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

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadePrompt(visible));
    }

    private IEnumerator FadePrompt(bool fadeIn)
    {
        float startAlpha = promptGroup.alpha;
        float endAlpha = fadeIn ? 1f : 0f;
        float t = 0f;

        if (fadeIn)
        {
            promptCanvas.gameObject.SetActive(true);
            progressBase.enabled = true;
            progressFill.enabled = true;

            if (progressHandle != null)
            {
                var handleImg = progressHandle.GetComponent<Image>();
                if (handleImg != null) handleImg.enabled = true;
            }
        }

        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float normalized = t / fadeDuration;
            promptGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, normalized);
            yield return null;
        }

        promptGroup.alpha = endAlpha;

        if (!fadeIn)
        {
            promptCanvas.gameObject.SetActive(false);
            progressBase.enabled = false;
            progressFill.enabled = false;

            if (progressHandle != null)
            {
                var handleImg = progressHandle.GetComponent<Image>();
                if (handleImg != null) handleImg.enabled = false;
            }
        }

        fadeCoroutine = null;
    }

    private void OnMovieEnded(VideoPlayer vp)
    {
        if (hasSkipped) return;
        hasSkipped = true;
        FadeOutCanvas.FadeOutExit();
    }

    private void SkipCutscene()
    {
        if (hasSkipped) return;
        hasSkipped = true;
        FadeOutCanvas.FadeOutExit();
    }
}