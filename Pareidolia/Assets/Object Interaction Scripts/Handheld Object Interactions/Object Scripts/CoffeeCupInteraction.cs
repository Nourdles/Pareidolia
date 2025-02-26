using UnityEngine;

public class CoffeeCupInteraction : HandheldObjectInteraction
{
    [SerializeField] private FMODUnity.EventReference mugPickupSFX;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cup;

        pickupSFX = mugPickupSFX;
    }
}
