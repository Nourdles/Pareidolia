using UnityEngine;
/// <summary>
/// Controls fog effect when the player is in the shower.
/// </summary>

public class ShowerFogController : MonoBehaviour
{
    private ParticleSystem fogParticles;
    private bool hasStartedShower = false;

    private ParticleSystem.EmissionModule emission;

    void Start()
    {
        fogParticles = GetComponent<ParticleSystem>();

        if (fogParticles != null)
        {
            emission = fogParticles.emission;
            emission.rateOverTime = 0; // keep fog off initially
        }
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += EnableFog;
        TubInteraction.GetIntoTubEvent += MarkShowerStarted;
        ShowerTask.ShowerComplete += DisableFog;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= EnableFog;
        TubInteraction.GetIntoTubEvent -= MarkShowerStarted;
        ShowerTask.ShowerComplete -= DisableFog;
    }

    private void MarkShowerStarted()
    {
        hasStartedShower = true;
    }

    private void EnableFog()
    {
        if (hasStartedShower) return; // only activate fog once

        if (fogParticles != null)
        {
            emission.rateOverTime = 50f;
        }
    }

    private void DisableFog()
    {
        if (fogParticles != null)
        {
            emission.rateOverTime = 0;
        }
    }
}
