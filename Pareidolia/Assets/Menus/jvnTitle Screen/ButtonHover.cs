using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public TMP_Text text;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public float fadeDuration = 0.1f;

    private Coroutine fadeCoroutine;
    private bool isSelected = false; // tracks if button is currently selected

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
        StartFade(highlightColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelected) // only reset if it's not selected
        {
            StartFade(normalColor);
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
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
        float time = 0;

        while (time < fadeDuration)
        {
            text.color = Color.Lerp(startColor, targetColor, time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }

        text.color = targetColor;
    }

    // reset ALLLLLL button colors when switching menus
    public void ResetTextColor()
    {
        isSelected = false;
        StartFade(normalColor);
    }
}
