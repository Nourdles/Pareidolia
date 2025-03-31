using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BowlInteraction : ObjectInteraction
{
    private String instruction;
    [SerializeField] private bool hasMilk = false;
    [SerializeField] private bool hasCereal = false;
    [SerializeField] private Material cerealOnlyMaterial;
    [SerializeField] private GameObject FullCerealPrefab;

    // FMOD events for pouring cereal and milk
    [SerializeField] private FMODUnity.EventReference cerealPourSFX;
    [SerializeField] private FMODUnity.EventReference milkPourSFX;

    public static event Action BreakfastMadeEvent;
    public static event Action<Material> ChangeBowlMat;

    protected override void Start()
    {
        base.Start();
        task = taskManager.GetComponentInChildren<MakeBreakfastTask>();
    }
    protected override void interactaction(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            Handhelds handheld_id = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld_id == Handhelds.Milk)
            {
                if (hasMilk)
                {
                    InvokeDialoguePromptEvent("I already added milk");
                } else if (hasCereal) // no milk
                {
                    hasMilk = true;
                    AudioManager.instance.PlayOneShot(milkPourSFX, transform.position);
                    Instantiate(FullCerealPrefab, transform.position, transform.rotation);
                    BreakfastMadeEvent?.Invoke();
                    Destroy(gameObject);
                } else // no cereal, no milk
                {
                    InvokeDialoguePromptEvent("Milk first?? No way!");
                }
            } else if (handheld_id == Handhelds.Cereal)
            {
                if (hasCereal)
                {
                    InvokeDialoguePromptEvent("I already put enough cereal in");
                } else
                {
                    hasCereal = true;
                    AudioManager.instance.PlayOneShot(cerealPourSFX, transform.position);
                    gameObject.GetComponent<MeshRenderer>().material = cerealOnlyMaterial;
                    ChangeBowlMat?.Invoke(cerealOnlyMaterial);
                }
            } else
            {
                MissingRequiredObject();
            }
        } else
        {
            MissingRequiredObject();
        }
    }

    protected override void UpdateInteractText()
    {
        if (interactText != "")
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + instruction;
        }
    }

    private void MissingRequiredObject()
    {
        if (!hasCereal && !hasMilk) // no cereal or milk
        {
            InvokeDialoguePromptEvent("I want cereal for breakfast");
        } else if (!hasMilk)
        {
            InvokeDialoguePromptEvent("I need to add milk");
        }
    }

    private void HoldingCerealInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to add cereal";
        instruction = "\"> to add cereal";
    }

    private void HoldingMilkInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pour milk";
        instruction = "\"> to pour milk";
    }

    void OnEnable()
    {
        PlayerInteract.DropItemEvent += ResetInteractionText;
        CerealInteraction.CerealPickupEvent += HoldingCerealInteractText;
        MilkInteraction.MilkPickupEvent += HoldingMilkInteractText;
    }

    void OnDisable()
    {
        PlayerInteract.DropItemEvent -= ResetInteractionText;
        CerealInteraction.CerealPickupEvent -= HoldingCerealInteractText;
        MilkInteraction.MilkPickupEvent -= HoldingMilkInteractText;
    }
}
