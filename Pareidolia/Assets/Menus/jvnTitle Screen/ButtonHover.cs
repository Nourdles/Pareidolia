using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;
using FMODUnity;

public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text text;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public float fadeDuration = 0.1f;

    private Coroutine fadeCoroutine;
    private bool isSelected = false;

    void Start()
    {
        if (text == null)
        {
            text = GetComponentInChildren<TMP_Text>();
        }

        text.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Play UI Hover sound when the pointer enters the UI element
        RuntimeManager.PlayOneShot("event:/Music/UI Hover");

        StartFade(highlightColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected)
        {
            StartFade(normalColor);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        RuntimeManager.PlayOneShot("event:/Music/UI Hover");

        isSelected = true;
        StartFade(highlightColor);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        isSelected = false;
        StartFade(normalColor);
    }

    private void StartFade(Color targetColor)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeTextColor(targetColor));
    }

    private IEnumerator FadeTextColor(Color targetColor)
    {
        Color startColor = text.color;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            text.color = Color.Lerp(startColor, targetColor, time / fadeDuration);
            yield return null;
        }

        text.color = targetColor;
    }

    public void ResetTextColor()
    {
        isSelected = false;

        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
            fadeCoroutine = null;
        }

        if (text != null)
        {
            text.color = normalColor;
        }
    }
}