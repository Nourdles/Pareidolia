using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SofaInteraction : ObjectInteraction
{
    public static event Action TVStartEvent; // notify that the tv task has been started
    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
        task = taskManager.GetComponentInChildren<TVTask>();
    }
    protected override void interactaction(GameObject objectInHand)
    {
        SetUninteractable();
        TVStartEvent?.Invoke();
        SceneSwitcher.LoadSceneOnTop("TVWatch");
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
    }
}
