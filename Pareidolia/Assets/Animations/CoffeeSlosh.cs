using UnityEngine;

public class CoffeeSlosh : MonoBehaviour
{
    public float sloshAmount = 0.1f;
    public float sloshSpeed = 3f;

    private Quaternion originalRotation;

    void Start()
    {
        originalRotation = transform.localRotation;
    }

    void Update()
    {
        float sloshX = Mathf.Sin(Time.time * sloshSpeed) * sloshAmount;
        float sloshZ = Mathf.Cos(Time.time * sloshSpeed) * sloshAmount;
        transform.localRotation = originalRotation * Quaternion.Euler(sloshX, 0, sloshZ);
    }
}
