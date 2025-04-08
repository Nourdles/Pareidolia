using UnityEngine;
using UnityEngine.InputSystem;

public class ShowerWaterController : MonoBehaviour
{
    private ParticleSystem showerParticles;
    private InputAction interactKey;
    private bool inShower = false;
    private bool waterStarted = false;

    void Start()
    {
        showerParticles = GetComponent<ParticleSystem>();
        interactKey = InputSystem.actions.FindAction("Interact");

        if (showerParticles != null)
        {
            showerParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    void OnEnable()
    {
        ShowerInteraction.GetIntoTubEvent += () => inShower = true;
        ShowerTask.ShowerComplete += () =>
        {
            inShower = false;
            StopWaterEffect();
        };
    }

    void OnDisable()
    {
        ShowerInteraction.GetIntoTubEvent -= () => inShower = true;
        ShowerTask.ShowerComplete -= () =>
        {
            inShower = false;
            StopWaterEffect();
        };
    }

    void Update()
    {
        if (!inShower || interactKey == null) return;

        if (interactKey.WasPressedThisFrame())
        {
            StartWaterEffect();
        }
        else if (interactKey.WasReleasedThisFrame())
        {
            StopWaterEffect();
        }
    }

    private void StartWaterEffect()
    {
        if (!waterStarted && showerParticles != null)
        {
            showerParticles.Play();
            waterStarted = true;
        }
    }

    private void StopWaterEffect()
    {
        if (waterStarted && showerParticles != null)
        {
            showerParticles.Stop();
            waterStarted = false;
        }
    }
}
