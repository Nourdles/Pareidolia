using UnityEngine;
using System.Collections;
using System;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// Script to make the basement light flicker and a silhouette appear and slowly sink into the ground once the player hits the trigger.
/// </summary>

public class SilhouetteFlickerEvent : MonoBehaviour
{
    [SerializeField] private Light flickerLight;  // light
    [SerializeField] private GameObject silhouetteSprite;  // silhouette
    [SerializeField] private int flickerCount = 5; // how many times it flickers
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float minY = -6f; // final y position
    [SerializeField] private float moveAmount = 0.7f; // final y position
    [SerializeField] private float flickerSpeed = 0.2f;
    [SerializeField] private EventReference basementStainSFX;

    public static event Action<string> SilhouetteDialogueEvent;
    public static event Action EventStart; // this event has started
    public static event Action EventEnd; // this event has finished

    private bool hasTriggered = false;
    void Start()
    {
        if (silhouetteSprite != null)
        {
            SetSpriteOpacity(silhouetteSprite, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && IsPlayer(other.gameObject))
        {
            hasTriggered = true;
            EventStart?.Invoke();

            RuntimeManager.PlayOneShot(basementStainSFX, transform.position);

            StartCoroutine(FlickerEffect());
            //BasementDoorDialogueEvent?.Invoke("What the hell is that?");
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0; // check layer
    }

    private IEnumerator FlickerEffect()
{

    Vector3 startPos = silhouetteSprite.transform.position;
    Vector3 targetPos = new Vector3(startPos.x, minY, startPos.z);

    for (int i = 0; i < flickerCount; i++)
    {
        flickerLight.enabled = !flickerLight.enabled;

        if (silhouetteSprite != null)
        {
            SetSpriteOpacity(silhouetteSprite, flickerLight.enabled ? 1f : 0f);

            if (silhouetteSprite.transform.position.y > minY) // move it down
            {
                Vector3 newPos = silhouetteSprite.transform.position;
                newPos.y -= moveAmount; 
                silhouetteSprite.transform.position = newPos;
            }
        }

        yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.4f));
    }

    flickerLight.enabled = true; // light on after event
    if (silhouetteSprite != null)
    {
        SetSpriteOpacity(silhouetteSprite, 0f);
    }

        // scripted event has ended
        // wait a bit 
        yield return new WaitForSeconds(1.5f);

        //SilhouetteDialogueEvent?.Invoke("Eugh...It's disgusting down here. Now I really need that shower.");
        SilhouetteDialogueEvent?.Invoke("Eugh...how did this happen? It's disgusting down here.");


        EventEnd?.Invoke();
}


    private void SetSpriteOpacity(GameObject spriteObject, float alpha)
    {
        SpriteRenderer sr = spriteObject.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color color = sr.color;
            color.a = alpha;
            sr.color = color;
        }
    }

}
