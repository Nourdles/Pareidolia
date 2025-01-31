using UnityEngine;
using System.Collections;

/// <summary>
/// Script for moving a face along a wall to follow the player's X-coordinate when they aren't looking.
/// The face fades in at the start, moves only when not being observed, and stays stationary when the player looks at it.
/// Uses raycasting to detect whether the player camera is looking.
/// </summary>
public class FaceMover : MonoBehaviour
{
    public SpriteRenderer faceRenderer;
    public Sprite faceSprite;
    public Transform playerCamera;
    public float moveSpeed = 2f;
    public float detectRange = 10f;
    public RandomFaceSpawner faceSpawner;

    private Vector3 originalPosition;
    private bool isPlayerLooking = false;
    private bool isMoving = false;

    void Start()
    {
        if (faceSpawner == null || faceRenderer == null || playerCamera == null)
        {
            Debug.LogError("Missing references in FaceMover!");
            return;
        }

        faceRenderer.sprite = faceSprite;
        faceRenderer.color = new Color(1f, 1f, 1f, 0f); // start fully transparent
        originalPosition = transform.position;

        StartCoroutine(faceSpawner.FadeIn(faceRenderer));
    }

    void Update()
    {
        CheckPlayerView();

        if (!isPlayerLooking && !isMoving)
        {
            StartCoroutine(MoveTowardsPlayerX());
        }
    }

    void CheckPlayerView()
    {
        Vector3 directionToFace = (transform.position - playerCamera.position).normalized;
        float angleToFace = Vector3.Angle(playerCamera.forward, directionToFace);
        Debug.Log($"Angle to face: {angleToFace}");

        if (angleToFace > 70f) // adjust FOV threshold here
        {
            isPlayerLooking = false;
            Debug.Log("Player is NOT looking at the face (out of FOV).");
            return;
        }

        RaycastHit hit;
        Debug.DrawRay(playerCamera.position, directionToFace * detectRange, Color.red);

        if (Physics.Raycast(playerCamera.position, directionToFace, out hit, detectRange))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");

            if (hit.collider != null && hit.collider.gameObject == gameObject)
            {
                isPlayerLooking = true;
                isMoving = false;
                Debug.Log("Player is looking at the face.");
                return;
            }
        }

        isPlayerLooking = false;
        Debug.Log("Player is NOT looking at the face.");
    }

    IEnumerator MoveTowardsPlayerX()
    {
        isMoving = true;
        Vector3 targetPosition = new Vector3(playerCamera.position.x, originalPosition.y, originalPosition.z);

        while (!isPlayerLooking && Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            yield return null;
        }

        isMoving = false;
    }
}
