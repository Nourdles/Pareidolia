using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

public class ClosedCurtainInteraction : ObjectInteraction
{
    [SerializeField] EventReference showerCurtainSfx;
    public static event Action OpenCurtainEvent;
    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open curtain";
    }

    protected override void interactaction(GameObject objectInHand)
    {
        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);
        OpenCurtainEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to open curtain";
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
