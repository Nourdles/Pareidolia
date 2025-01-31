using TMPro;
using UnityEngine;

public class SanityDisplayScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public TextMeshProUGUI text;
    public GameObject faceManager;

    private SanityTracker tracker;
    void Start()
    {
        tracker = faceManager.GetComponent<SanityTracker>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        text.text = tracker.getSanity().ToString() + "%";

    }
}
