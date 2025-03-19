using System.Collections;
using UnityEngine;

public class FridgeSilhouetteEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer silhouetteSprite;
    [SerializeField] private float fadeDuration = 1.5f; // Fade in time
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetLocalX = 1.4f; // Where it moves in LOCAL space

    private bool hasFadedIn = false;
    private bool hasMoved = false;

    void Start()
    {
        // Ensure the silhouette starts fully invisible
        if (silhouetteSprite != null)
        {
            Color color = silhouetteSprite.color;
            color.a = 0f;
            silhouetteSprite.color = color;
        }
    }

    void OnEnable()
    {
        FridgeDoorInteraction.OnFirstFridgeOpen += FadeInSilhouette;
        FridgeDoorInteraction.OnFirstFridgeClose += MoveSilhouette;
    }

    void OnDisable()
    {
        FridgeDoorInteraction.OnFirstFridgeOpen -= FadeInSilhouette;
        FridgeDoorInteraction.OnFirstFridgeClose -= MoveSilhouette;
    }

    private void FadeInSilhouette()
    {
        if (!hasFadedIn)
        {
            hasFadedIn = true;
            StartCoroutine(FadeIn());
        }
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        Color color = silhouetteSprite.color;
        float targetAlpha = 0.6f; // opacity

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeDuration);
            silhouetteSprite.color = color;
            yield return null;
        }
    }

    private void MoveSilhouette()
    {
        if (!hasMoved)
        {
            hasMoved = true;
            StartCoroutine(MoveToLocalPosition(targetLocalX));
        }
    }

    private IEnumerator MoveToLocalPosition(float xTargetLocal)
    {
        Vector3 startLocalPosition = transform.localPosition; // Use local position
        Vector3 targetLocalPosition = new Vector3(xTargetLocal, startLocalPosition.y, startLocalPosition.z); // Keep Y, Z same

        while (Mathf.Abs(transform.localPosition.x - xTargetLocal) > 0.01f) // Ensure only X changes
        {
            transform.localPosition = new Vector3(
                Mathf.MoveTowards(transform.localPosition.x, xTargetLocal, moveSpeed * Time.deltaTime),
                startLocalPosition.y,  // Lock Y position
                startLocalPosition.z   // Lock Z position
            );

            yield return null;
        }
    }
}
