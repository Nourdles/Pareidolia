using System;
using UnityEngine;

public class OpenCurtainInteraction : ObjectInteraction
{
    public static event Action CloseCurtainEvent;
    public override void interact(GameObject objectInHand)
    {
        InvokeCloseCurtain();
    }

    private void InvokeCloseCurtain()
    {
        CloseCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += InvokeCloseCurtain;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= InvokeCloseCurtain;
    }
}
