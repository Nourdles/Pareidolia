using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;
using static UnityEngine.UI.Image;

public class DeathManager : MonoBehaviour
{

    public Image blackPanel;

    public static event Action<string> DeathEvent;

    public GameObject Player;

    public GameObject SpawnPoint;

    public GameObject StainPrefab;

    public Sprite bodySprite;

    public MoveCamera cameraMovement;

    public GameObject cameraPosition;

    public static event Action RespawnEvent;
    public static event Action DeathSceneEvent;

    [SerializeField] private CharacterController cc;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blackPanel.color = new Color(0, 0, 0, 0);
    }

    IEnumerator waitNSeconds(float n)
    {
        yield return new WaitForSeconds(n);
    }

    void WaitForSecondsManual(float seconds)
    {
        float start = Time.time;
        while (Time.time < start + seconds)
        {
            // Do nothing, just wait
        }
    }

    private void blackScreen()
    {
        blackPanel.color = new Color(0, 0, 0, 255);
    }

    private void disableBlackScreen()
    {
        blackPanel.color = new Color(0, 0, 0, 0);
    }

    private void SpawnDeathStain(RaycastHit hit)
    {
        Vector3 pos = hit.point;
        Vector3 normal = hit.normal;
        GameObject newStain = Instantiate(StainPrefab, pos + (normal * 0.05f), Quaternion.identity);

        newStain.transform.rotation = Quaternion.LookRotation(-normal);

        newStain.transform.localScale = new Vector3(0.5f, 0.5f, 1f);

        SpriteRenderer sr = newStain.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = bodySprite;
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1f);
        }

    }


    private void SpawnStainAtPlayerPosition()
    {
        RaycastHit floorHit;


        RaycastHit[] hits = Physics.RaycastAll(Player.transform.position, Vector3.down, 35f);
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider == null) continue;
            if (hit.collider.CompareTag("Wood"))
            {
                Debug.Log("Spawning stain");
                SpawnDeathStain(hit);
                break;
            }
        }


    }

    private IEnumerator MoveToPositionAndRotation(Transform transform, Vector3 position, Quaternion rotation, float duration)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        Vector3 endPos = position;
        Quaternion endRot = rotation;

        float time = 0f;
        while (time < duration)
        {
            float t = time / duration;
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            time += Time.deltaTime;
            yield return null;
        }

        // Final snap to make sure it's exact
        transform.position = endPos;
        transform.rotation = endRot;
    }

    public IEnumerator ProcessDeath(GameObject murderingStain, Vector3 normal)
    {
        DeathSceneEvent?.Invoke();
        DeathEvent?.Invoke("I feel dizzy. What's going on??");
        SanityTracker.damageEnabled = false;


        SpawnStainAtPlayerPosition();
        cc.enabled = false;
        cameraMovement.enabled = false;

        //Store relative position to player
        Vector3 originalLocalPosition = cameraPosition.transform.localPosition;
        Quaternion originalLocalRotation = cameraPosition.transform.localRotation;
        Transform originalParent = cameraPosition.transform.parent;

        //Retrieve position on ceiling


        RaycastHit[] hits = Physics.RaycastAll(Player.transform.position, Vector3.up, 30f);

        RaycastHit hit = hits[0];
        bool foundCeiling = false;
        foreach (RaycastHit h in hits)
        {
            if (h.collider.CompareTag("Ceiling"))
            {
                hit = h;
                foundCeiling = true;
                break;
            }
        }
        //Only if we can
        if (foundCeiling)
        {
            Vector3 pos = hit.point + new Vector3(0, -1f, 0);
            //Detach camera from player and move it to stain
            StartCoroutine(MoveToPositionAndRotation(cameraPosition.transform, pos, Quaternion.LookRotation(hit.normal), 5f));
            yield return new WaitForSeconds(5f);

        }

        blackScreen();

        Player.transform.position = SpawnPoint.transform.position;

        cameraPosition.transform.parent = originalParent;
        cameraPosition.transform.localPosition = originalLocalPosition;
        cameraPosition.transform.localRotation = originalLocalRotation;
        yield return new WaitForSeconds(3);

        disableBlackScreen();
        SanityTracker.damageEnabled = true;
        cc.enabled = true;
        cameraMovement.enabled = true;

        DeathEvent?.Invoke("What a weird dream");
        RespawnEvent?.Invoke();

    }
}
