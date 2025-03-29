using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GlowHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Image hoverGlowImage;
    public float fadeSpeed = 5f;
    public float moveDistance = 10f;

    private Color originalColor;
    private Vector2 originalPosition;
    private Coroutine hoverRoutine;

    void Start()
    {
        if (hoverGlowImage == null)
        {
            Debug.LogWarning("GlowHoverEffect: No image assigned!");
            return;
        }

        originalColor = hoverGlowImage.color;
        originalColor.a = 0f;
        hoverGlowImage.color = originalColor;

        originalPosition = hoverGlowImage.rectTransform.anchoredPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TriggerGlowIn();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TriggerGlowOut();
    }

    public void OnSelect(BaseEventData eventData)
    {
        TriggerGlowIn();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TriggerGlowOut();
    }

    void TriggerGlowIn()
    {
        if (hoverRoutine != null) StopCoroutine(hoverRoutine);
        hoverRoutine = StartCoroutine(FadeInGlow());
    }

    void TriggerGlowOut()
    {
        if (hoverRoutine != null) StopCoroutine(hoverRoutine);
        hoverRoutine = StartCoroutine(FadeOutGlow());
    }

    IEnumerator FadeInGlow()
    {
        float t = 0;
        Vector2 targetPos = originalPosition + Vector2.up * moveDistance;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;

            Color c = hoverGlowImage.color;
            c.a = Mathf.Lerp(0f, 1f, t);
            hoverGlowImage.color = c;

            hoverGlowImage.rectTransform.anchoredPosition = Vector2.Lerp(originalPosition, targetPos, t);

            yield return null;
        }
    }

    IEnumerator FadeOutGlow()
    {
        float t = 0;
        Vector2 startPos = hoverGlowImage.rectTransform.anchoredPosition;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;

            Color c = hoverGlowImage.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            hoverGlowImage.color = c;

            hoverGlowImage.rectTransform.anchoredPosition = Vector2.Lerp(startPos, originalPosition, t);

            yield return null;
        }
    }
}