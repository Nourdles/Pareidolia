using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class IntroSequenceDialogueEvent : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int dialogueTriggerNum;
    public static event Action<string> IntroDialogueEvent;
    [SerializeField] private Volume postProcessingVolume;

    private FilmGrain filmGrain;
    private DepthOfField depthOfField;
    private Coroutine distortionRoutine;

    private void Start()
    {
        if (postProcessingVolume != null)
        {
            postProcessingVolume.profile.TryGet(out filmGrain);
            postProcessingVolume.profile.TryGet(out depthOfField);

            if (filmGrain != null)
                filmGrain.intensity.overrideState = true;

            if (depthOfField != null)
                depthOfField.focusDistance.overrideState = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            if (dialogueTriggerNum == 1)
            {
                IntroDialogueEvent?.Invoke("What is that?");

                // damage effect to teach player
                if (filmGrain != null && depthOfField != null)
                {
                    if (distortionRoutine != null)
                        StopCoroutine(distortionRoutine);

                    distortionRoutine = StartCoroutine(AnimateDistortion());
                }
                // do not disable immediately – will be handled at end of coroutine
            }
            else if (dialogueTriggerNum == 2)
            {
                IntroDialogueEvent?.Invoke("My head hurts...");
                // disable trigger so it cant be triggered again
                gameObject.SetActive(false);
            }
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }

    private IEnumerator AnimateDistortion()
    {
        float grainMin = 0.1f;
        float grainMax = 1f;
        float dofMin = 0.1f;
        float dofMax = 10f;
        float fadeDuration = 0.4f;
        float holdDuration = 1.5f;

        // fade in
        float timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            filmGrain.intensity.value = Mathf.Lerp(grainMin, grainMax, t);
            depthOfField.focusDistance.value = Mathf.Lerp(dofMax, dofMin, t);
            timer += Time.deltaTime;
            yield return null;
        }

        filmGrain.intensity.value = grainMax;
        depthOfField.focusDistance.value = dofMin;

        // hold
        yield return new WaitForSeconds(holdDuration);

        // fade out
        timer = 0f;
        while (timer < fadeDuration)
        {
            float t = timer / fadeDuration;
            filmGrain.intensity.value = Mathf.Lerp(grainMax, grainMin, t);
            depthOfField.focusDistance.value = Mathf.Lerp(dofMin, dofMax, t);
            timer += Time.deltaTime;
            yield return null;
        }

        filmGrain.intensity.value = grainMin;
        depthOfField.focusDistance.value = dofMax;
        distortionRoutine = null;

        // NOW disable the object
        gameObject.SetActive(false);
    }
}
