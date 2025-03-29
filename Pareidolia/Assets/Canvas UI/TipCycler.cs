using UnityEngine;
using TMPro;

public class TipCycler : MonoBehaviour
{
    public TMP_Text tipText;

    [TextArea(2, 4)]
    public string[] tips;
    public float interval = 5f; // 5-second cycle

    private int currentTipIndex = 0;
    private float timer;

    void Start()
    {
        if (tips.Length == 0 || tipText == null)
        {
            Debug.LogWarning("TipCycler: Missing tip text or tips array.");
            enabled = false;
            return;
        }

        tipText.text = tips[0];
        timer = interval;
    }

    void Update()
    {
        timer -= Time.unscaledDeltaTime;

        if (timer <= 0f)
        {
            currentTipIndex = (currentTipIndex + 1) % tips.Length;
            tipText.text = tips[currentTipIndex];
            timer = interval;
        }
    }
}
