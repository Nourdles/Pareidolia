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

    void Start()
    {
        // hide stains
        livingRoomStains.SetActive(false);
    }


    // fade in stains behind couch as player is watching tv
    public void StartEvent()
    {
        StartCoroutine(FadeInStain(TVStain));
    }

    private IEnumerator FadeInStain(SpriteRenderer stainRenderer)
    {
        float stainAlpha = stainRenderer.color.a;
        Color temp = stainRenderer.color;

        while (stainRenderer.color.a > 0)
        {
            stainAlpha -= fadeInRate;
            temp.a = stainAlpha;
            stainRenderer.color = temp;

            yield return new WaitForSeconds(0.05f);
        }
        livingRoomStains.SetActive(true);
    }


    void ViewingStains()
    {   
        RandomFaceSpawner.EnableFaceSpawning();
        LivingRoomDialogueEvent.Invoke("What is this? God...my head hurts...");

        // once player takes damage for the first time
        //LivingRoomDialogueEvent.Invoke("God! My head...");
        // when player leaves basement:

    }

}
