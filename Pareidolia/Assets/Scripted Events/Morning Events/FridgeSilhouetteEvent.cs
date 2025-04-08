using System.Collections;
using UnityEngine;

public class FridgeSilhouetteEvent : MonoBehaviour
{
    [SerializeField] private SpriteRenderer silhouetteSprite;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float targetLocalX = 1.4f;
    [SerializeField] private GameObject silhouetteObject;
    [SerializeField] private LayerMask playerLayer;

    private bool hasMoved = false;

    private void Start()
    {
        if (silhouetteSprite != null)
        {
            Color color = silhouetteSprite.color;
            color.a = 0.6f;
            silhouetteSprite.color = color;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasMoved && IsPlayer(other.gameObject))
        {
            hasMoved = true;
            StartCoroutine(MoveToLocalPosition(targetLocalX));
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }

    private IEnumerator MoveToLocalPosition(float xTargetLocal)
    {
        Transform silhouetteTransform = silhouetteObject.transform;
        Vector3 startLocalPosition = silhouetteTransform.localPosition;
        Vector3 targetLocalPosition = new Vector3(xTargetLocal, startLocalPosition.y, startLocalPosition.z);

        while (Mathf.Abs(silhouetteTransform.localPosition.x - xTargetLocal) > 0.01f)
        {
            silhouetteTransform.localPosition = new Vector3(
                Mathf.MoveTowards(silhouetteTransform.localPosition.x, xTargetLocal, moveSpeed * Time.deltaTime),
                startLocalPosition.y,
                startLocalPosition.z
            );

            yield return null;
        }
    }
}
