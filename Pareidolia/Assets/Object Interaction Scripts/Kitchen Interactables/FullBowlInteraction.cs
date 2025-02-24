using System;
using Unity.VisualScripting;
using UnityEngine;

public class FullBowlInteraction : ObjectInteraction
{
    [SerializeField] private int bites = 0;
    [SerializeField] private int max_bites = 5;
    [SerializeField] private GameObject EmptyBowlPrefab;
    public static event Action EatCerealEvent;

    public override void interact(GameObject objectInHand)
    {
        
        bites += 1;
        EatCerealEvent?.Invoke(); // for eating sfx
        if (bites >= max_bites) // at all the cereal
        {
            Instantiate(EmptyBowlPrefab, transform.position, transform.rotation);
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
