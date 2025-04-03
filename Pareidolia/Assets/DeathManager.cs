using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;
using Unity.VisualScripting;

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

    private IEnumerator SpawnDeathStain(RaycastHit hit)
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
        yield break;

    }


    private IEnumerator SpawnStainAtPlayerPosition()
    {
        RaycastHit floorHit;
        if (Physics.Raycast(Player.transform.position, Vector3.down, out floorHit, 30f))
        {
            if(floorHit.collider.CompareTag("Wood"))
            {
                StartCoroutine(SpawnDeathStain(floorHit));
            }
        }
        yield break;
    }

    public IEnumerator ProcessDeath(GameObject murderingStain, Vector3 normal)
    {
        DeathEvent?.Invoke("I feel dizzy. What's going on??");

        blackScreen();


        StartCoroutine(SpawnStainAtPlayerPosition());
        cc.enabled = false;
        cameraMovement.enabled = false;

        //Store relative position to player
        Vector3 originalLocalPosition = cameraPosition.transform.localPosition;
        Quaternion originalLocalRotation = cameraPosition.transform.localRotation;
        Transform originalParent = cameraPosition.transform.parent;

        //Detach camera from player and move it to stain
        cameraPosition.transform.parent = null;
        cameraPosition.transform.position = murderingStain.transform.position;
        cameraPosition.transform.rotation = Quaternion.LookRotation(normal);

        yield return new WaitForSeconds(1.2f);


        //Player.transform.position = murderingStain.transform.position;

        disableBlackScreen();

        yield return new WaitForSeconds(5f);


        blackScreen();

        Player.transform.position = SpawnPoint.transform.position;

        cameraPosition.transform.parent = originalParent;
        cameraPosition.transform.localPosition = originalLocalPosition;
        cameraPosition.transform.localRotation = originalLocalRotation;
        yield return new WaitForSeconds(3);

        disableBlackScreen();
        cc.enabled = true;
        cameraMovement.enabled = true;

        DeathEvent?.Invoke("What a weird dream");

    }
}
