public class MilkInteraction : HandheldObjectInteraction
{
    public FMODUnity.EventReference milkPickupSFX;
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Milk;

        pickupSFX = milkPickupSFX;
    }
}
