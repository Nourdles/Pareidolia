using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class TextColorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public float fadeDuration = 0.1f;

    private Coroutine fadeCoroutine;

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TMP_Text>();
        }
        text.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeTextColor(highlightColor));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeTextColor(normalColor));
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
}