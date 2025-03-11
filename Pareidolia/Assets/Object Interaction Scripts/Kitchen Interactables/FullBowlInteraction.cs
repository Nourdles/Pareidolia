using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FullBowlInteraction : ObjectInteraction
{
    [SerializeField] private int bites = 0;
    [SerializeField] private int max_bites = 5;
    [SerializeField] private GameObject EmptyBowlPrefab;
    public static event Action EatCerealEvent;

    protected override void Start()
    {
        base.Start();
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to eat cereal";
    }
    public override void interact(GameObject objectInHand)
    {
        if (objectInHand != null)
        {
            Handhelds handheld = objectInHand.GetComponent<HandheldObjectInteraction>().getHandheld();
            if (handheld == Handhelds.Spoon)
            {
                bites += 1;
                EatCerealEvent?.Invoke(); // for eating sfx
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
}
