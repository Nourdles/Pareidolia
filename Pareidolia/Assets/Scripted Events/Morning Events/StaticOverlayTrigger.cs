using UnityEngine;
using UnityEngine.UI;


public class StaticOverlayTrigger : MonoBehaviour
{
    [SerializeField] private Image staticOverlayImg;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int dialogueTriggerNum;
    [SerializeField] private Transform player;
    [SerializeField] private Transform hallwayExitDoor;

    private float prevDistance;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        prevDistance = Vector3.Distance(gameObject.transform.position, hallwayExitDoor.transform.position);
        // set opacity of static animation to be 0
        Color staticColor = staticOverlayImg.color;
        staticColor.a = 0f;
        staticOverlayImg.color = staticColor;
        staticOverlayImg.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        // get distance between player and door at the end of the exit sequence
        float distance = Vector3.Distance(player.transform.position, hallwayExitDoor.transform.position);
        if (distance < prevDistance)
        {
            Color staticColor = staticOverlayImg.color;
            float alpha = Mathf.Clamp(staticColor.a + 0.0001f, 0f, 0.2f);
            staticColor.a = alpha;
            staticOverlayImg.color = staticColor;
            prevDistance = distance;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            staticOverlayImg.enabled = true;
            //}
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }
}
