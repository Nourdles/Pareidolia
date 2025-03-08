using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;

/// <summary>
/// Script for spawning faces on walls within the player's FOV, and despawning them once the player looks away (using Raycasting)
/// The position within the FOV, number of faces, and fade-in time are randomized within a given range.
/// If Raycasting detects an object not tagged as "wall", faces won't appear (avoid furniture obstructions)
/// </summary>

public class RandomFaceSpawner : MonoBehaviour
{
    public GameObject facePrefab;   
    public Sprite[] faceSprites;
    public SanityTracker sanityTracker;
    public Camera playerCamera;
    public int maxFacesPerSpot = 3; // max faces in area looked at
    public int maxTotalFaces = 10; // max faces in scene
    public float fadeInTime = 2f;
    public float maxOpacity = 1.5f;
    public float fadeOutTime = 2f;

    private int totalFaceCount = 0;

    private int testCount = 3; //Delete this
    private List<GameObject> activeFaces = new List<GameObject>(); // list of active faces
    
    private static Coroutine co = null;
    static bool faceSpawnOn = false;
    public bool spawnInFOV = false;
    public float fovAngle = 90f;
   /* void Start()
    {
        if (playerCamera == null) return;
        StartCoroutine(SpawnFacesRandomly());
    } */
    public static void EnableFaceSpawning()
    {
        faceSpawnOn = true;
        Debug.Log("Face Spawning Enabled");
    }

    public static void DisableFaceSpawning()
    {
        faceSpawnOn = false;
        Debug.Log("Face Spawning Disabled");
    }

    void Update()
    {

        if (faceSpawnOn && co == null)
        {
            co = (spawnInFOV) ? StartCoroutine(SpawnFacesRandomly()) : StartCoroutine(SpawnFacesOutOfFOV());
        }
        else if (!faceSpawnOn && co != null)
        {
            StopCoroutine(co);
            co = null;
        }
        CheckPlayerView();
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
                UnityEngine.ColorUtility.TryParseHtmlString("#C1B89F", out Color yellowTint); // yellow tint to blend with wall
                sr.color = new Color(yellowTint.r, yellowTint.g, yellowTint.b, 0f);
            }

            activeFaces.Add(newFace);
            sanityTracker.registerStain(newFace);
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

    public IEnumerator FadeIn(SpriteRenderer sr)
    {
        if (sr == null) yield break;

        float alpha = 0f;
        UnityEngine.ColorUtility.TryParseHtmlString("#C1B89F", out Color color);
        color.a = 0f;

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

    /*void Update()
    {
        CheckPlayerView();
    } */

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
        return;
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

    //Confirms validity of sprite position, clamps to be within surface (position passed in is a ref)
    //Note: only works for x,y,z aligned walls
    bool IsSpritePositionValid(Collider collider, ref Vector3 spritePosition, Vector2 spriteSize, Vector3 normal)
    {
        normal = normal.normalized; // Ensure normal is unit length
        Bounds bounds = collider.bounds;

        Vector3 surface = bounds.size;
        Vector3 min = bounds.min, max = bounds.max;
        float reqWidth = spriteSize.x, reqHeight = spriteSize.y;

        // Determine which axis surface is normal to (x, y, z)
        // Determine width & height axes based on normal
        int w, h;
        if (Mathf.Abs(normal.x) > 0.999f) // X-aligned wall
        {
            w = 2; h = 1; // Width along Z, Height along Y
        }
        else // normal.y or normal.z dominant
        {
            w = 0; h = (Mathf.Abs(normal.y) > 0.999f) ? 2 : 1; // Width along X, Height along Y/Z
        }

        float surfaceW = surface[w];
        float surfaceH = surface[h];

        if (surfaceW < reqWidth || surfaceH < reqHeight)
            return false; // Surface too small

        // Adjust position to keep the sprite inside the surface bounds
        float halfW = reqWidth / 2, halfH = reqHeight / 2;
        spritePosition[w] = Mathf.Clamp(spritePosition[w], min[w] + halfW, max[w] - halfW);
        spritePosition[h] = Mathf.Clamp(spritePosition[h], min[h] + halfH, max[h] - halfH);

        return true;
    }

    bool isSurfaceBigEnough(Collider collider, Vector2 size, Vector3 normal)
    {
        normal = normal.normalized;
        Vector3 surface = collider.bounds.size;
        float reqWidth = size.x, reqHeight = size.y, surfaceW = 0, surfaceH = 0;
        if (Mathf.Abs(normal.x) > 0.9f)
        {
            surfaceW = surface.z; surfaceH = surface.y;
        }
        else if (Mathf.Abs(normal.y) > 0.9f)
        {
            surfaceW = surface.x; surfaceH = surface.z;
        }
        else if (Mathf.Abs(normal.z)  > 0.9f)
        {
            surfaceW = surface.x; surfaceH = surface.y;
        }
        return surfaceW >= reqWidth && surfaceH >= reqHeight;
    }

    IEnumerator SpawnFace(RaycastHit hit)
    {
        Vector3 position = hit.point;
        Vector3 normal = hit.normal;
        float delay = 0;
        yield return new WaitForSeconds(delay);

        //Check if spawn position is too close to existing faces
        foreach (GameObject existingFace in activeFaces)
        {
            if (existingFace != null && Vector3.Distance(existingFace.transform.position, position) < 1f)
            {
                yield break;
            }
        }


        float randomScale = Random.Range(0.02f, 0.06f); // random size
        Sprite sprite = faceSprites[Random.Range(0, faceSprites.Length)]; //Get a sprite
        Vector2 finalSize = sprite.bounds.size * randomScale; 

        if(!IsSpritePositionValid(hit.collider, ref position, finalSize, normal))
        {
            yield break;
        }

        GameObject newFace = Instantiate(facePrefab, position + (normal * 0.01f), Quaternion.identity);
        newFace.transform.rotation = Quaternion.LookRotation(-normal);

        newFace.transform.localScale = new Vector3(randomScale, randomScale, 1f);

        SpriteRenderer sr = newFace.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sprite = faceSprites[Random.Range(0, faceSprites.Length)];
            UnityEngine.ColorUtility.TryParseHtmlString("#C1B89F", out Color yellowTint); // yellow tint to blend with wall
            sr.color = new Color(yellowTint.r, yellowTint.g, yellowTint.b, 0f);
        }
        activeFaces.Add(newFace);
        sanityTracker.registerStain(newFace);
        totalFaceCount++;

        StartCoroutine(FadeIn(sr));
    }
 
    IEnumerator SpawnFacesOutOfFOV()
    {
        while (true)
        {
            if (totalFaceCount < maxTotalFaces)
            {
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
                RaycastHit hit;
                Vector3 randomDirection = randomDirectionOutsideFOV(playerCamera.transform.forward);
                if (Physics.Raycast(playerCamera.transform.position, randomDirection, out hit))
                {
                    if (hit.collider.CompareTag("Wall"))
                    {
                        StartCoroutine(SpawnFace(hit));
                    }
                }

            } else
            {
                yield return new WaitForSeconds(Random.Range(5f, 10f));
                //Remove a random face
                while (true)
                {
                    int idx = Random.Range(0, activeFaces.Count);
                    Debug.Log(idx.ToString() + " " +  activeFaces.Count.ToString());
                    GameObject face = activeFaces[idx];
                    if (face != null)
                    {
                        StartCoroutine(FadeOutAndDestroy(face));
                        break;
                    }
                }
            }

        }
    }

    Vector3 randomDirectionOutsideFOV(Vector3 playerForward)
    {
        float halfFovCos = Mathf.Cos(fovAngle * 0.5f * Mathf.Deg2Rad);

        while (true)
        {
            Vector3 randomDir = Random.onUnitSphere;
            if (Vector3.Dot(playerForward, randomDir) < halfFovCos)
            {
                return randomDir;
            }
        }
    }
}
