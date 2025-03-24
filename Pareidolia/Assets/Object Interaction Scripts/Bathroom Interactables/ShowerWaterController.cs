using UnityEngine;

public class ShowerWaterController : MonoBehaviour
{
    private ParticleSystem showerParticles;

    void Start()
    {
        showerParticles = GetComponent<ParticleSystem>();

        if (showerParticles != null)
        {
            showerParticles.Stop(); // Keep it off initially
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
        if (showerParticles != null && !showerParticles.isPlaying)
        {
            showerParticles.Play();
        }
    }

    private void StopWaterEffect()
    {
        if (showerParticles != null && showerParticles.isPlaying)
        {
            showerParticles.Stop();
        }
    }
}
