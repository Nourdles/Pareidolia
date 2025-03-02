using UnityEngine;

public class CerealInteraction : HandheldObjectInteraction
{
    [SerializeField] private FMODUnity.EventReference cerealPickupSFX;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cereal;

        pickupSFX = cerealPickupSFX;
    }
}
