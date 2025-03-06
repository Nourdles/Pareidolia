using System.Collections;
using UnityEngine;

public class ShowerSilhouetteEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer silhouetteSprite; // Assign in Inspector
    [SerializeField] private float fadeDuration = 2f; // Slower fade-out
    [SerializeField] private float moveSpeed = 5f; // Faster movement
    [SerializeField] private float moveDistance = 2f; // Distance to move forward
    [SerializeField] private float startAlpha = 0.3f; // Lower initial opacity

    private bool hasMoved = false;

    void Start()
    {
        // Ensure it starts with correct visibility
        if (silhouetteSprite != null)
        {
            Color color = silhouetteSprite.color;
            color.a = startAlpha; // Starts semi-transparent
            silhouetteSprite.color = color;
        }
    }

    void OnEnable()
    {
        ClosedCurtainInteraction.OpenCurtainEvent += MoveAndFadeSilhouette;
    }

    void OnDisable()
    {
        ClosedCurtainInteraction.OpenCurtainEvent -= MoveAndFadeSilhouette;
    }

    private void MoveAndFadeSilhouette()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            StartCoroutine(MoveForwardAndFade());
        }
    }

    private IEnumerator MoveForwardAndFade()
    {
        Vector3 startLocalPosition = transform.localPosition;
        Vector3 targetLocalPosition = startLocalPosition + (Vector3.forward * moveDistance); // Moves forward in local space

        Debug.Log($"Moving Silhouette: Start Local Z={startLocalPosition.z}, Target Local Z={targetLocalPosition.z}");

        float moveTime = 0.75f; // Move quickly
        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startLocalPosition, targetLocalPosition, elapsedTime / moveTime);
            yield return null;
        }

        Debug.Log("Silhouette reached target position.");
        yield return new WaitForSeconds(0.2f); // Short delay before fading

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        Color color = silhouetteSprite.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            silhouetteSprite.color = color;
            yield return null;
        }

        silhouetteSprite.gameObject.SetActive(false); // Remove silhouette after fade
        Debug.Log("Silhouette faded out.");
    }
}
