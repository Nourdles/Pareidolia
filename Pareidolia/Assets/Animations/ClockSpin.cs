using UnityEngine;

/// <summary>
/// Spins the clock hands wildly after deterioration
/// </summary>
public class ClockSpin : MonoBehaviour
{
    private enum State { Spinning, Swaying }

    public float minSpinSpeed = 60f;
    public float maxSpinSpeed = 180f;
    public float minSpinAngle = 270f;
    public float maxSpinAngle = 360f;

    public int minSways = 2;
    public int maxSways = 5;
    public float swayAngle = 10f;
    public float swaySpeed = 3f;
    private float targetSpinAngle;

    private State currentState = State.Spinning;

    private float spinSpeed;
    private float currentSpin;
    private int spinDirection; // 1 or -1

    private Quaternion initialLocalRotation;

    private int swaysRemaining;
    private float swayTimer;
    private float swayDuration;
    private float swayStartAngle;
    private float swayTargetAngle;
    private bool swayFinalTowardSpin = false;
    private bool animationActive = false;

    void Start()
    {
        initialLocalRotation = transform.localRotation;
        animationActive = false;
    }

    void Update()
    {
        if (!animationActive) return;

        switch (currentState)
        {
            case State.Spinning:
                PerformSpin();
                break;
            case State.Swaying:
                PerformSway();
                break;
        }
    }

    public void StartAnimation()
    {
        if (!animationActive)
        {
            animationActive = true;
            BeginNewSpin();
        }
    }

    void BeginNewSpin()
    {
        currentState = State.Spinning;
        spinSpeed = Random.Range(minSpinSpeed, maxSpinSpeed);
        spinDirection = Random.value < 0.5f ? 1 : -1;
        currentSpin = 0f;
        targetSpinAngle = Random.Range(minSpinAngle, maxSpinAngle);
    }

    void PerformSpin()
    {
        float deltaAngle = spinDirection * spinSpeed * Time.deltaTime;
        currentSpin += Mathf.Abs(deltaAngle); // track total distance regardless of direction

        transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(currentSpin * spinDirection, Vector3.forward);

        if (currentSpin >= targetSpinAngle)
        {
            BeginSway();
        }
    }

    void BeginSway()
    {
        currentState = State.Swaying;
        swaysRemaining = Random.Range(minSways, maxSways + 1);
        swayStartAngle = currentSpin * spinDirection;
        swayTargetAngle = swayStartAngle + swayAngle * spinDirection;
        swayTimer = 0f;
        swayDuration = 1f / swaySpeed;
        swayFinalTowardSpin = true; // final sway should end in spin direction
    }

    void PerformSway()
    {
        swayTimer += Time.deltaTime;
        float t = Mathf.Clamp01(swayTimer / swayDuration);
        float angle = Mathf.Lerp(swayStartAngle, swayTargetAngle, t);
        transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(angle, Vector3.forward);

        if (t >= 1f)
        {
            swayTimer = 0f;
            swaysRemaining--;

            if (swaysRemaining <= 0)
            {
                // Final sway direction: end toward spin direction
                swayStartAngle = swayTargetAngle;
                BeginNewSpin();
            }
            else
            {
                float temp = swayStartAngle;
                swayStartAngle = swayTargetAngle;
                swayTargetAngle = temp;
            }
        }
    }
}
