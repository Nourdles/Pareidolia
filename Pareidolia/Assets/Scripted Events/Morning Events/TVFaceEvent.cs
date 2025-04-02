using UnityEngine;
using System.Collections;
using System;
public class TVFaceEvent : MonoBehaviour
{
    [SerializeField] GameObject livingRoomStains;
    //[SerializeField] RandomFaceSpawner faceSpawner;
    [SerializeField] SpriteRenderer TVStain;
    public static event Action<string> LivingRoomDialogueEvent;
    [SerializeField] float fadeInRate = 0.01f;
    [SerializeField] new Camera camera;
    [SerializeField] SanityTracker sanityTracker;

    void Start()
    {
        // hide stains
        livingRoomStains.SetActive(false);
    }


    // fade in stains in front of TV and behind couch as player is watching tv
    public void StartEvent()
    {
        StartCoroutine(FadeInStain(TVStain));
    }

    private IEnumerator FadeInStain(SpriteRenderer stainRenderer)
    {
        float stainAlpha = stainRenderer.color.a;
        Color temp = stainRenderer.color;

        while (stainRenderer.color.a < 1)
        {
            stainAlpha += fadeInRate;
            temp.a = stainAlpha;
            stainRenderer.color = temp;

            yield return new WaitForSeconds(0.05f);
        }

        // set the stains behind the couch to active
        livingRoomStains.SetActive(true);
        // enable face spawning 
        RandomFaceSpawner.EnableFaceSpawning();
        // add stain to sanity tracker so player takes damage when looking
        sanityTracker.registerStain(livingRoomStains);
    }


    // Check if the player is viewing the wall stains
    /*void ViewingStains()
    {   

        //RandomFaceSpawner.EnableFaceSpawning();
        //LivingRoomDialogueEvent.Invoke("What is this? God...my head hurts...");

        // once player takes damage for the first time
        //LivingRoomDialogueEvent.Invoke("God! My head...");
        // when player leaves basement:

        // Reused code from Sanity tracker
        var cameraPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
        if (!GeometryUtility.TestPlanesAABB(cameraPlanes, livingRoomStains.GetComponent<Collider>().bounds))
        {

            Vector3 toObject = (livingRoomStains.transform.position - camera.transform.position);
            float distance = toObject.magnitude;
            Vector3 direction = toObject / distance;

            if (Physics.Raycast(camera.transform.position, direction, out RaycastHit hit, distance))
            {
                if (hit.collider.gameObject == livingRoomStains)
                {
                    {
                        LivingRoomDialogueEvent.Invoke("What is this? God...my head hurts...");
                    }
                }
            }
        }

    }*/

}
