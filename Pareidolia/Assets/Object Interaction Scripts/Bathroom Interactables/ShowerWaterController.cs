using UnityEngine;
///<summary>
///script to run a water particle effect when the player is doing the shower task. should have a cooldown to avoid race condition
///</summary>
public class ShowerWaterController : MonoBehaviour
{
    private ParticleSystem showerParticles;
    private bool isWaterRunning = false; // track if water should be running
    private float cooldownTime = 0.2f; // small delay to prevent rapid toggling
    private float lastToggleTime = -1f;

    void Start()
    {
        showerParticles = GetComponent<ParticleSystem>();

        if (showerParticles != null)
        {
            showerParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear); 
        }
    }

    void OnEnable()
    {
        Shower.ShowerOnEvent += StartWaterEffect;
        Shower.ShowerOffEvent += StopWaterEffect;
    }

    void OnDisable()
    {
        Shower.ShowerOnEvent -= StartWaterEffect;
        Shower.ShowerOffEvent -= StopWaterEffect;
    }

    private void StartWaterEffect()
    {
        if (showerParticles == null) return;
        
        float currentTime = Time.time;
        if (currentTime - lastToggleTime < cooldownTime) return;

        lastToggleTime = currentTime;
        isWaterRunning = true;

        if (!showerParticles.isPlaying)
        {
            showerParticles.Play();
        }
    }

    private void StopWaterEffect()
    {
        if (showerParticles == null) return;

        float currentTime = Time.time;
        if (currentTime - lastToggleTime < cooldownTime) return;

        lastToggleTime = currentTime;
        isWaterRunning = false;

        if (showerParticles.isPlaying)
        {
            showerParticles.Stop();
        }
    }
}
