using UnityEngine;
using System.Collections;

public class FridgeLightFlicker : MonoBehaviour
{
    public Light fridgeLight;
    public float minIntensity = 0.8f;
    public float maxIntensity = 1.2f;
    public float flickerDuration = 0.2f;
    public float flickerCooldownMin = 2f; // min time between flickers
    public float flickerCooldownMax = 5f; // max time between flickers

    private float originalIntensity;

    void Start()
    {
        if (fridgeLight == null)
        {
            fridgeLight = GetComponent<Light>();
        }

        if (fridgeLight != null)
        {
            originalIntensity = fridgeLight.intensity;
            StartCoroutine(FlickerRoutine());
        }
        else
        {
            Debug.LogError("No Light component found on " + gameObject.name);
        }
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(flickerCooldownMin, flickerCooldownMax));

            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            float flickerTime = 0f;

            while (flickerTime < flickerDuration)
            {
                flickerTime += Time.deltaTime;
                fridgeLight.intensity = Mathf.Lerp(fridgeLight.intensity, targetIntensity, flickerTime / flickerDuration);
                yield return null;
            }

            fridgeLight.intensity = originalIntensity;
        }
    }
}
