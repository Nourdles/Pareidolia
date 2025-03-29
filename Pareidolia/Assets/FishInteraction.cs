using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FishInteraction : ObjectInteraction
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        base.Start();
        interactText = "shader test";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void UpdateInteractText()
    {
        interactText = "works";
    }

    public override void interact(GameObject objectInHand)
    {
        
    }

}
