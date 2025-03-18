using UnityEngine;
using UnityEngine.InputSystem;

public class SofaInteraction : ObjectInteraction
{
    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
    }
    public override void interact(GameObject objectInHand)
    {
        SceneSwitcher.LoadSceneOnTop("TVWatch");
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
    }
}
