using UnityEngine;
using System.Collections;
using System;
using System.Linq;
using NUnit.Framework.Constraints;
public class TVFaceEvent : MonoBehaviour
{
    [SerializeField] GameObject livingRoomStains;
    //[SerializeField] RandomFaceSpawner faceSpawner;
    [SerializeField] SpriteRenderer TVStain;
    [SerializeField] SpriteRenderer EyeStain;
    public static event Action<string> LivingRoomDialogueEvent;
    [SerializeField] float FadeInDelay = 3f;
    [SerializeField] float TVFadeInRate = 0.002f;
    [SerializeField] float EyeFadeInRate = 0.0015f;
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
        // fading in the TV stain slowly
        Color tvColor = TVStain.color;
        tvColor.a = 0f;
        TVStain.color = tvColor;
        StartCoroutine(FadeInStain(TVStain, TVFadeInRate));
        // fade in eye stain as well
        Color EyeColor = EyeStain.color;
        EyeColor.a = 0f;
        EyeStain.color = EyeColor;
        StartCoroutine(FadeInStain(EyeStain, EyeFadeInRate));

        // fading in all other living room stains faster
        livingRoomStains.SetActive(true);
        StartCoroutine(FadeInLivingRoomStains());
    }

    private IEnumerator FadeInLivingRoomStains()
    {
        yield return new WaitForSeconds(FadeInDelay);
        SpriteRenderer[] stains = livingRoomStains.GetComponentsInChildren<SpriteRenderer>(true);

        foreach (var sr in stains)
        {
            Color c = sr.color;
            c.a = 0f;
            sr.color = c;
        }

        float alpha = 0f;
        float fasterFadeRate = TVFadeInRate * 1f; 

        while (alpha < 1f)
        {
            alpha += fasterFadeRate;
            alpha = Mathf.Clamp01(alpha);

            foreach (var sr in stains)
            {
                if (sr != null)
                {
                    Color c = sr.color;
                    c.a = alpha;
                    sr.color = c;
                }
            }

            yield return new WaitForSeconds(0.05f);
        }

        foreach (var sr in stains)
        {
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f;
                sr.color = c;
            }
        }

        // Enable sanity interaction once fully faded
        sanityTracker.registerStain(livingRoomStains);
    }

    private IEnumerator FadeInStain(SpriteRenderer stainRenderer, float fadeInRate)
    {
        // wait a bit before starting to fade in stains
        yield return new WaitForSeconds(FadeInDelay);
        float stainAlpha = stainRenderer.color.a;
        Color temp = stainRenderer.color;

        while (stainRenderer.color.a < 1)
        {
            stainAlpha += fadeInRate;
            temp.a = stainAlpha;
            stainRenderer.color = temp;

            yield return new WaitForSeconds(0.05f);
        }

        // enable face spawning 
        //RandomFaceSpawner.EnableFaceSpawning();
        // add stain to sanity tracker so player takes damage when looking
        //sanityTracker.registerStain(livingRoomStains);
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
