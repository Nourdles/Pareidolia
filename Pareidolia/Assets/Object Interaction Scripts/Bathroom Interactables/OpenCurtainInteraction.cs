using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

public class OpenCurtainInteraction : ObjectInteraction
{
    public static event Action CloseCurtainEvent;
    [SerializeField] EventReference showerCurtainSfx;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close curtain";
    }
    protected override void interactaction(GameObject objectInHand)
    {
        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);
        InvokeCloseCurtain();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to close curtain";
    }

    private void InvokeCloseCurtain()
    {
        CloseCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        ShowerInteraction.GetIntoTubEvent += InvokeCloseCurtain;
    }

    void OnDisable()
    {
        ShowerInteraction.GetIntoTubEvent -= InvokeCloseCurtain;
    }
}
