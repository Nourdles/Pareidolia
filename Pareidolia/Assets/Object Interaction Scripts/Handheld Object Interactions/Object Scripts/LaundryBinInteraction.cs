using UnityEngine;

public class LaundryBinInteraction : HandheldObjectInteraction
{
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.LaundryBin;
    }
}
