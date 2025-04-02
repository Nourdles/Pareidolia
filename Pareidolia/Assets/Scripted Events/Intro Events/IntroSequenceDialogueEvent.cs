using System;
using UnityEngine;

public class IntroSequenceDialogueEvent : MonoBehaviour
{

    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int dialogueTriggerNum;
    public static event Action<string> IntroDialogueEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            if (dialogueTriggerNum == 1)
            {
                IntroDialogueEvent?.Invoke("What is that?");
                // disable trigger so it cant be triggered again
                gameObject.SetActive(false);
            }
            else if (dialogueTriggerNum == 2)
            {
                IntroDialogueEvent?.Invoke("My head hurts...");
                // disable trigger so it cant be triggered again
                gameObject.SetActive(false);
            }
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }


}
