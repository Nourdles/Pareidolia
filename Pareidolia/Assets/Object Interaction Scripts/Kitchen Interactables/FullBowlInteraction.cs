using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using FMODUnity;

public class FullBowlInteraction : ObjectInteraction
{
    [SerializeField] private int bites = 0;
    [SerializeField] private int max_bites = 5;
    [SerializeField] private GameObject EmptyBowlPrefab;
    [SerializeField] private string cerealEatSFXPath = "event:/SFX/Cereal Eat";

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to eat cereal";
    }
    protected override void interactaction(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            Handhelds handheld = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld == Handhelds.Spoon)
            {
                RuntimeManager.PlayOneShot(cerealEatSFXPath, transform.position);
                bites += 1;
                InvokeDialoguePromptEvent("Yummy!");

                if (bites >= max_bites) // at all the cereal
                {
                    Instantiate(EmptyBowlPrefab, transform.position, transform.rotation);
                    Destroy(gameObject.transform.parent.gameObject);
                } 
            } else
            {
                InvokeDialoguePromptEvent("I can't eat cereal with this");
            } 
        } else
        {
            InvokeDialoguePromptEvent("I need my spoon to eat this");
        }
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to eat cereal";
    }
}
