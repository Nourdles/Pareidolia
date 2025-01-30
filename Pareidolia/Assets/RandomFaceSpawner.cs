using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Script for spawning faces on walls within the player's FOV, and despawning them once the player looks away (using Raycasting)
/// The position within the FOV, number of faces, and fade-in time are randomized within a given range.
/// If Raycasting detects an object not tagged as "wall", faces won't appear (avoid furniture obstructions)
/// </summary>

public class RandomFaceSpawner : MonoBehaviour
{
    public GameObject facePrefab;   
    public Sprite[] faceSprites;
    public Camera playerCamera;
    public int maxFacesPerSpot = 3; // max faces in area looked at
    public int maxTotalFaces = 10; // max faces in scene
    public float fadeInTime = 2f;
    public float maxOpacity = 0.5f;
    public float fadeOutTime = 2f;

    private int totalFaceCount = 0;
    private List<GameObject> activeFaces = new List<GameObject>(); // list of active faces

    void Start()
    {
        if (playerCamera == null) return;
        StartCoroutine(SpawnFacesRandomly());
    }

    IEnumerator SpawnFacesRandomly()
    {
        while (true) // check if we can spawn more faces
        {
            if (totalFaceCount < maxTotalFaces)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                RaycastHit hit;
                Vector3 randomDirection = playerCamera.transform.forward + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.3f, 0.3f), 0);
                if (Physics.Raycast(playerCamera.transform.position, randomDirection, out hit, 30f))
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        StartCoroutine(SpawnMultipleFaces(hit.point, hit.normal));
                    }
                }
            }
            else
            {
                yield return null; //w ait for active faces to decrease
            }
        }
    }

    IEnumerator SpawnMultipleFaces(Vector3 position, Vector3 normal)
    {
        int faceCount = 0;
        float delay = 0;

        while (faceCount < maxFacesPerSpot && totalFaceCount < maxTotalFaces)
        {
            yield return new WaitForSeconds(delay);
            delay += Random.Range(0.5f, 2f);

            Vector3 randomOffset = GenerateRandomOffsetInFOV(normal, position); // make sure they don't overlap
            Vector3 spawnPosition = position + randomOffset;

            // check if the spawn position is too close to existing faces
            bool isTooClose = false;

            foreach (GameObject existingFace in activeFaces)
            {
                if (existingFace != null && Vector3.Distance(existingFace.transform.position, spawnPosition) < 1f)
                {
                    isTooClose = true;
                    break;
                }
            }

            if (isTooClose) continue;

            // check if the spawn position is obstructed by furniture
            if (Physics.Raycast(playerCamera.transform.position, spawnPosition - playerCamera.transform.position, out RaycastHit obstructionHit, 30f))
            {
                if (!obstructionHit.collider.CompareTag("Wall"))
                {
                    continue;
                }
            }

            // new face at the calculated position
            GameObject newFace = Instantiate(facePrefab, spawnPosition + (normal * 0.01f), Quaternion.identity);
            newFace.transform.rotation = Quaternion.LookRotation(-normal);

            float randomScale = Random.Range(0.02f, 0.06f); // random size
            newFace.transform.localScale = new Vector3(randomScale, randomScale, 1f);

            SpriteRenderer sr = newFace.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = faceSprites[Random.Range(0, faceSprites.Length)];
                sr.color = new Color(1f, 1f, 1f, 0f);
            }

            activeFaces.Add(newFace);
            totalFaceCount++;
            faceCount++;

            StartCoroutine(FadeIn(sr));
        }
    }

    Vector3 GenerateRandomOffsetInFOV(Vector3 normal, Vector3 position)
    {
        Vector3 tangent = Vector3.Cross(normal, Vector3.up).normalized;
        Vector3 bitangent = Vector3.Cross(normal, tangent).normalized;

        float randomX = Random.Range(-2f, 2f);
        float randomY = Random.Range(-2f, 2f);

        return (tangent * randomX) + (bitangent * randomY);
    }

    IEnumerator FadeIn(SpriteRenderer sr)
    {
        if (sr == null) yield break;

        float alpha = 0f;
        Color color = sr.color;

        while (alpha < maxOpacity)
        {
            if (sr == null || sr.gameObject == null) yield break;
            alpha += Time.deltaTime / fadeInTime;
            alpha = Mathf.Clamp01(alpha);
            color.a = alpha;
            sr.color = color;
            yield return null;
        }

        color.a = maxOpacity;
        if (sr != null) sr.color = color;
    }

    void Update()
    {
        CheckPlayerView();
    }

    void CheckPlayerView()
    {
        Vector3[] rayDirections = {
            playerCamera.transform.forward,
            playerCamera.transform.forward + new Vector3(0.1f, 0, 0), // right
            playerCamera.transform.forward + new Vector3(-0.1f, 0, 0), // left
            playerCamera.transform.forward + new Vector3(0, 0.1f, 0), // up
            playerCamera.transform.forward + new Vector3(0, -0.1f, 0) // down
        };

        foreach (Vector3 direction in rayDirections)
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, direction.normalized, out hit, 30f))
            {
                if (hit.collider.CompareTag("Wall"))
                {
                    return; // if any ray hits a wall, do nothing (we're already looking at a wall)
                }
            }
        }

        StartFadingOutFaces();
    }

    void StartFadingOutFaces()
    {
        for (int i = activeFaces.Count - 1; i >= 0; i--)
        {
            GameObject face = activeFaces[i];
            if (face != null)
            {
                StartCoroutine(FadeOutAndDestroy(face));
            }
        }
    }

    IEnumerator FadeOutAndDestroy(GameObject face)
    {
        if (face == null) yield break;

        SpriteRenderer sr = face.GetComponent<SpriteRenderer>();
        if (sr == null) yield break;

        float alpha = sr.color.a;
        Color color = sr.color;

        while (alpha > 0f)
        {
            if (face == null || sr == null) yield break;

            alpha -= Time.deltaTime / fadeOutTime;
            alpha = Mathf.Clamp01(alpha);
            color.a = alpha;
            sr.color = color;

            yield return null;
        }

        if (face != null)
        {
            Destroy(face);
            activeFaces.Remove(face);
            totalFaceCount--;         
        }
    }
}
