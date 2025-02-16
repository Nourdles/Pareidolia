using System;

public class BedInteraction: ObjectInteraction
{
    public static event Action BedInteractionEvent;

    protected override void InvokeInteractionEvent()
    {
        BedInteractionEvent?.Invoke();
    }
}
