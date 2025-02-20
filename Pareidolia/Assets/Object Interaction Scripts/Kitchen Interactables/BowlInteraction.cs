using System;
using UnityEngine;

public class BowlInteraction : ObjectInteraction
{
    [SerializeField] private bool hasMilk = false;
    [SerializeField] private bool hasCereal = false;
    public static event Action EatCerealEvent;
    public static event Action BreakfastMadeEvent;
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
                    // change to full cereal bowl model
                    // invoke subtask completion 
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
                    // change texture to have cereal + complete subtask
                    BreakfastMadeEvent?.Invoke();
                }
            } else if (handheld_id == Handhelds.Spoon)
            {
                if (hasCereal && hasMilk)
                {
                    // play eating sound + change bowl back to empty vers
                    EatCerealEvent?.Invoke();
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
