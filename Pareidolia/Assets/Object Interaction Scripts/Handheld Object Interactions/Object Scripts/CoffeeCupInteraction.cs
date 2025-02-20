public class CoffeeCupInteraction : HandheldObjectInteraction
{
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Cup;
    }
}
