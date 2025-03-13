using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenuFader : MonoBehaviour
{
    public CanvasGroup mainMenuCanvasGroup; // controls fade-in of UI buttons
    public Button firstButton; // first button to auto-select after fade-in
    public float fadeDuration = 1.0f; // time for fade-in animation
    private bool hasFadedIn = false; // prevents multiple fade-ins

    void Start()
    {
        // hide MainMenu UI buttons at start
        mainMenuCanvasGroup.alpha = 0f;
        mainMenuCanvasGroup.interactable = false;
        mainMenuCanvasGroup.blocksRaycasts = false;
    }

    public void StartFadeIn()
    {
        if (!hasFadedIn)
        {
            hasFadedIn = true; // prevent fade-in from happening again
            StartCoroutine(FadeInMainMenu());
        }
        else
        {
            // buttons are interactable even if fade was skipped
            mainMenuCanvasGroup.alpha = 1f;
            mainMenuCanvasGroup.interactable = true;
            mainMenuCanvasGroup.blocksRaycasts = true;
            SelectFirstButton();
        }
    }

    IEnumerator FadeInMainMenu()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            mainMenuCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            yield return null;
        }

        // UI is fully visible & interactable after fade
        mainMenuCanvasGroup.alpha = 1f;
        mainMenuCanvasGroup.interactable = true;
        mainMenuCanvasGroup.blocksRaycasts = true;

        // auto select first button after fade-in
        SelectFirstButton();
    }

    void SelectFirstButton()
    {
        if (firstButton != null)
        {
            EventSystem.current.SetSelectedGameObject(firstButton.gameObject);
        }
    }
}
