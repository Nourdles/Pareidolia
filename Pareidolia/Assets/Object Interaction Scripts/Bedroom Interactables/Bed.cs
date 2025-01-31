using System;

public class Bed: InteractableObject
{
    public static event Action BedInteraction;
    protected override void interact()
    {
        BedInteraction?.Invoke();
    }
}
