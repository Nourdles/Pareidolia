using System;
using UnityEngine;

public class ClosedCurtainInteraction : ObjectInteraction
{
    public static event Action OpenCurtainEvent;
    public override void interact(GameObject objectInHand)
    {
        OpenCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        OpenCurtainInteraction.CloseCurtainEvent += SetUninteractable;
        ShowerTask.ShowerComplete += SetInteractable;
    }

    void OnDisable()
    {
        OpenCurtainInteraction.CloseCurtainEvent -= SetUninteractable;
        ShowerTask.ShowerComplete -= SetInteractable;
    }
}
