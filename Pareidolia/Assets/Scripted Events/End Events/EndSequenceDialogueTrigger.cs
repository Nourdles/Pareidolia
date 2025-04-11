using UnityEngine;
using System;
public class EndDialogueTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int dialogueTriggerNum;
    [SerializeField] private GameObject wallBlocker;
    [SerializeField] private GameObject morningFaceManager;
    public static event Action<string> EndDialogueEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            //if (dialogueTriggerNum == 1)
            //{
                //EndDialogueEvent?.Invoke("Who's there?");
                // stop face spawning/sanity tracking to prevent softlock
                morningFaceManager.SetActive(false);
                // enable wall that blocks player from going back
                wallBlocker.SetActive(true);
                
                // disable trigger so it cant be triggered again
                gameObject.SetActive(false);

            //}
        }
    }

    private bool IsPlayer(GameObject obj)
    {
        return (playerLayer.value & (1 << obj.layer)) != 0;
    }
}
