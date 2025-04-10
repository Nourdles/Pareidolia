using UnityEngine;

public class FishBob : MonoBehaviour
{
    public float bobbingAmplitude = 0.05f;
    public float bobbingFrequency = 2f;

    private Vector3 basePosition;

    void Start()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
        transform.position = basePosition + new Vector3(0f, bob, 0f);
    }
}
