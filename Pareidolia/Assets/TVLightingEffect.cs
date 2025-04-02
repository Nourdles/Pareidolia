using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class TVLightingEffect : MonoBehaviour
{
    [Header("TV Lighting")]
    [SerializeField] private Light tvLight; // turns on immediately
    [SerializeField] private Light ceilingLight; // flickers after delay
    [SerializeField] private float delayBeforeFlicker = 10f;
    [SerializeField] private int flickerCountMin = 3;
    [SerializeField] private int flickerCountMax = 4;
    [SerializeField] private float flickerIntervalMin = 0.05f;
    [SerializeField] private float flickerIntervalMax = 0.3f;

    private VideoPlayer video;

    void Start()
    {
        video = GetComponent<VideoPlayer>();
        if (video != null)
        {
            video.started += OnVideoStarted;
        }
        else
        {
            Debug.LogError("no VideoPlayer found");
        }
    }

    void OnDestroy()
    {
        if (video != null)
        {
            video.started -= OnVideoStarted;
        }
    }

    private void OnVideoStarted(VideoPlayer vp)
    {
        if (tvLight != null)
        {
            tvLight.enabled = true;
        }

        StartCoroutine(FlickerCeilingLightAfterDelay());
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