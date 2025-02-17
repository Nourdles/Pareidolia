using System;
using UnityEngine;

public class HandheldObjectInteraction : ObjectInteraction
{
    protected InventoryManager inventoryManager;
    public static event Action<GameObject> PickUpEvent;
    public static event Action DropEvent;

    // add fxnality to caninteract()
    // uses base.Update()

    protected override void Start()
    {
        base.Start();
        inventoryManager = GameObject.FindWithTag("Inventory").GetComponent<InventoryManager>();
    }

    protected override void InvokeInteractionEvent()
    {
        if (inventoryManager.isHoldingObject())
        {

        }

        throw new System.NotImplementedException();
    }
}
