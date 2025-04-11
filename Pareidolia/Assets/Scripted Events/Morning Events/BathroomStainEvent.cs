using UnityEngine;

/// <summary>
/// while player is showering, reveal stains in bedroom + hallway
/// </summary>
public class BathroomStainEvent : MonoBehaviour
{
    //[SerializeField] SpriteRenderer SinkStain;
    //[SerializeField] SpriteRenderer FloorStain;
    //[SerializeField] SpriteRenderer BedroomStain;
    [SerializeField] GameObject bathroomEventStains;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bathroomEventStains.SetActive(false);
    }

    public void OnEnable()
    {
        ShowerInteraction.GetIntoTubEvent += StartEvent;
    }

    public void OnDisable()
    {
        ShowerInteraction.GetIntoTubEvent -= StartEvent;
    }

    void StartEvent()
    {
        bathroomEventStains.SetActive(true);
        // register stains in sanity checker? 
    }
}
