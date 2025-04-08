using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeurigInteraction : ObjectInteraction
{
    [SerializeField] private Transform cupHoldPointTransform;
    [SerializeField] private FMODUnity.EventReference coffeeMachineSFX;
    public static event Action CoffeeMadeEvent;
    //public static event Action CoffeeDrankEvent;
    public static event Action CupPutInMachineEvent;
    [SerializeField] private ParticleSystem coffeePourEffect;
    [SerializeField] private GameObject coffeeContent;
    private bool isPouring = false;

    protected override void Start()
    {
        base.Start();
        task = taskManager.GetComponentInChildren<MakeCoffeeTask>();
    }

    protected override void interactaction(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            Handhelds handheld_id = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld_id == Handhelds.Cup)
            {
                PutCupInMachine(objectInHand);
                // invoke after x seconds (time for coffee to complete)
                CoffeeMadeEvent?.Invoke();
                gameObject.tag = "Untagged"; // no longer interactable

            } else
            {
                InvokeDialoguePromptEvent("I need to get my coffee mug for this");
            }
        } else
        {
            InvokeDialoguePromptEvent("I need to get my coffee mug for this");
        }
    }

    private void PutCupInMachine(GameObject cup)
    {
        Rigidbody cupRb = cup.GetComponentInParent<Rigidbody>();
        cupRb.transform.parent = cupHoldPointTransform.transform;
        cupRb.isKinematic = false;
        cupRb.detectCollisions = true;

        GameObject cupCenter = cup.transform.parent.gameObject;
        cupCenter.transform.position = cupHoldPointTransform.position;
        cupCenter.transform.rotation = cupHoldPointTransform.rotation;

        AudioManager.instance.PlayOneShot(coffeeMachineSFX, transform.position);
        
        // set as interactable again
        if (coffeePourEffect != null)
        {
            coffeePourEffect.Play();
            isPouring = true;
            Invoke(nameof(StopPouring), 2f); // stop after 2 seconds
        }

        // set as interactable again
        cup.tag = "InteractableObject";
        cup.layer = LayerMask.NameToLayer("Default");
        CupPutInMachineEvent?.Invoke();
    }

    private void StopPouring()
    {
        if (isPouring && coffeePourEffect != null)
        {
            coffeePourEffect.Stop();
            isPouring = false;
        }

        // Show the coffee in the cup
        if (coffeeContent != null)
        {
            coffeeContent.SetActive(true);
        }
    }

    protected override void UpdateInteractText()
    {
        if (interactText != "")
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to place mug in the coffee machine";
        }
    }

    private void HoldingCupInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
        interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to place mug in the coffee machine";
    }

   void OnEnable()
    {
        CoffeeCupInteraction.CupPickupEvent += HoldingCupInteractText;
        PlayerInteract.DropItemEvent += ResetInteractionText;
    }

    void OnDisable()
    {
        CoffeeCupInteraction.CupPickupEvent -= HoldingCupInteractText;
        PlayerInteract.DropItemEvent -= ResetInteractionText;
        if (isPouring)
        {
            StopPouring();
        }
    }
}
