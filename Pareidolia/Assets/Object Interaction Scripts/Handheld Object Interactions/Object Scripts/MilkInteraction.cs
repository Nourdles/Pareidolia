public class MilkInteraction : HandheldObjectInteraction
{
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Milk;
    }
}
