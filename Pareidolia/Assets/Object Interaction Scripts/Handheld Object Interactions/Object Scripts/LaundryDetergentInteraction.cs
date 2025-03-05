using UnityEngine;

public class LaundryDetergentInteraction : HandheldObjectInteraction
{
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.Detergent;
    }
}
