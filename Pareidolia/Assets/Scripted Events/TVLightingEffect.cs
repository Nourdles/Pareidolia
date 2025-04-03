using System.Collections;
using UnityEngine;

public class TVLightingEffect : MonoBehaviour
{
    [SerializeField] private Light TVLight;
    [SerializeField] private Light ceilingLight;
    [SerializeField] private float delayBeforeFlicker = 10f;
    [SerializeField] private int flickerCountMin = 3;
    [SerializeField] private int flickerCountMax = 4;
    [SerializeField] private float flickerIntervalMin = 0.05f;
    [SerializeField] private float flickerIntervalMax = 0.3f;

    private bool hasStarted = false;

    void OnEnable()
    {
        SofaInteraction.TVStartEvent += OnTVStarted;
        TVSceneManager.TVWatchedEvent += OnTVEnded;
    }

    void OnDisable()
    {
        SofaInteraction.TVStartEvent -= OnTVStarted;
        TVSceneManager.TVWatchedEvent -= OnTVEnded;
    }

    private void OnTVStarted()
    {
        if (hasStarted) return;
        hasStarted = true;

        if (TVLight != null)
            TVLight.enabled = true;

        StartCoroutine(FlickerCeilingLightAfterDelay());
    }

    private void OnTVEnded()
    {
        if (TVLight != null)
            TVLight.enabled = false;
    }

    private IEnumerator FlickerCeilingLightAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeFlicker);

        if (ceilingLight == null) yield break;

        int flickers = Random.Range(flickerCountMin, flickerCountMax + 1);
        for (int i = 0; i < flickers; i++)
        {
            ceilingLight.enabled = false;
            yield return new WaitForSeconds(Random.Range(flickerIntervalMin, flickerIntervalMax));
            ceilingLight.enabled = true;
            yield return new WaitForSeconds(Random.Range(flickerIntervalMin, flickerIntervalMax));
        }

        ceilingLight.enabled = false;
    }
}
