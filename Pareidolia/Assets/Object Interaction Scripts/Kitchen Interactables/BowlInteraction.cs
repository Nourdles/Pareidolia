using System;
using UnityEngine;

public class BowlInteraction : ObjectInteraction
{
    [SerializeField] private bool hasMilk = false;
    [SerializeField] private bool hasCereal = false;
    [SerializeField] private Material cerealOnlyMaterial;
    [SerializeField] private GameObject FullCerealPrefab;
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
