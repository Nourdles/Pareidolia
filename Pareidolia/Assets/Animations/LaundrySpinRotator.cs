using UnityEngine;

public class LaundrySpinRotator : MonoBehaviour
{
    public float spinSpeed = 700f;
    private bool isSpinning = false;

    void Update()
    {
        if (isSpinning)
        {
            transform.Rotate(-Vector3.right * spinSpeed * Time.deltaTime);
        }
    }

    public void StartSpinning()
    {
        isSpinning = true;
    }

    public void StopSpinning()
    {
        isSpinning = false;
    }
}
