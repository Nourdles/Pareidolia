using UnityEngine;
using System;
public class EndDialogueTrigger : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private int dialogueTriggerNum;
    [SerializeField] private GameObject wallBlocker;
    public static event Action<string> EndDialogueEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other.gameObject))
        {
            //if (dialogueTriggerNum == 1)
            //{
                //EndDialogueEvent?.Invoke("Who's there?");
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
