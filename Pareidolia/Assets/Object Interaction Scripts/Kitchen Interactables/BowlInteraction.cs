using System;
using UnityEngine;

public class BowlInteraction : ObjectInteraction
{
    [SerializeField] private bool hasMilk = false;
    [SerializeField] private bool hasCereal = false;
    [SerializeField] private Material cerealOnlyMaterial;
    [SerializeField] private Material cerealMilkMat;
    public static event Action EatCerealEvent;
    public static event Action BreakfastMadeEvent;
    public static event Action<Material> ChangeBowlMat;
    public override void interact(GameObject objectInHand)
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
                    gameObject.GetComponent<MeshRenderer>().material = cerealMilkMat;
                    ChangeBowlMat?.Invoke(cerealMilkMat);
                    // change to full cereal bowl model + complete subtask
                    BreakfastMadeEvent?.Invoke();
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
                    gameObject.GetComponent<MeshRenderer>().material = cerealOnlyMaterial;
                    ChangeBowlMat?.Invoke(cerealOnlyMaterial);
                }
            } else if (handheld_id == Handhelds.Spoon)
            {
                if (hasCereal && hasMilk)
                {
                    // play eating sound + change bowl back to empty vers
                    EatCerealEvent?.Invoke();
                    gameObject.tag = "Untagged"; // no longer interactable
                } else 
                {
                    MissingRequiredObject();
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
}
