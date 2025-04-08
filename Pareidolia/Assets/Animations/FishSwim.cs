using UnityEngine;
using System.Collections;

/// <summary>
/// Make the fish bob idly in the tank and move in a figure eight at random intervals with random radius + speed and while switching animations.
/// </summary>
public class FishSwim : MonoBehaviour
{
    public float bobbingAmplitude = 0.05f;
    public float bobbingFrequency = 2f;
    public Transform centerPoint;

    private float radius;
    private float speed;
    private float time;
    private bool isMoving = false;
    private Vector3 baseCenter;
    private Animator animator;

    void Start()
    {
        if (centerPoint == null)
            centerPoint = transform;

        baseCenter = centerPoint.position;
        animator = GetComponent<Animator>();
        StartCoroutine(MovementLoop());
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;

        if (isMoving)
        {
            time += speed * Time.deltaTime;

            float x = Mathf.Sin(time) * radius;
            float z = Mathf.Sin(time * 2f) * 0.5f * radius;

            Vector3 newPos = baseCenter + new Vector3(x, bob, z);
            transform.position = newPos;

            // look ahead only on XZ to prevent tilt
            float lookAheadTime = time + 0.1f;
            float nextX = Mathf.Sin(lookAheadTime) * radius;
            float nextZ = Mathf.Sin(lookAheadTime * 2f) * 0.5f * radius;

            Vector3 lookTarget = baseCenter + new Vector3(nextX, 0f, nextZ);
            lookTarget.y = transform.position.y;

            transform.LookAt(lookTarget);
            transform.Rotate(0, 180f, 0); // flip to face -Z
        }
        else
        {
            // idle bobbing at center
            transform.position = baseCenter + new Vector3(0f, bob, 0f);
        }
    }

    IEnumerator MovementLoop()
    {
        while (true)
        {
            // pause & bob
            isMoving = false;
            if (animator != null) animator.Play("idle");
            yield return new WaitForSeconds(Random.Range(3f, 8f));

            // randomize radius + speed
            radius = Random.Range(0.2f, 0.46f);
            speed = Random.Range(2f, 5f);
            time = 0f;

            isMoving = true;
            if (animator != null) animator.Play("Moving");

            float movementDuration = (2 * Mathf.PI) / speed;
            yield return new WaitForSeconds(movementDuration);
        }
    }
}
