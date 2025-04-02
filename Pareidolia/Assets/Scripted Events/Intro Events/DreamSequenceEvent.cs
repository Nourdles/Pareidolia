using UnityEngine;
using System;
public class DreamSequenceEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DoorInteraction hallwayExitDoor;
    [SerializeField] FadeExitScene fadeOutCanvas;

    [SerializeField] private BoxCollider InputTutorialTrigger;
    [SerializeField] private BoxCollider DialogueTrigger;
    public static event Action<string> DreamDialogueEvent;


    public void OnEnable()
    {
        hallwayExitDoor.DoorFirstOpeningEvent += FadeOut;
        //DialogueTrigger.On
    }
    public void OnDisable()
    {
        hallwayExitDoor.DoorFirstOpeningEvent -= FadeOut;
    }
    public void FadeOut()
    {
        fadeOutCanvas.FadeOutAnim();
        DreamDialogueEvent?.Invoke("Another day.");
        Debug.Log("dialogue");


    }
}
