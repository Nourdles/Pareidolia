using UnityEngine;
using System;
public class EndSequenceManager : MonoBehaviour
{
    [SerializeField] DoorInteraction hallwayExitDoor;
    [SerializeField] FadeExitScene fadeOutCanvas;

    public static event Action<string> EndDialogueEvent;


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
        fadeOutCanvas.FadeOutExit();
        //DreamDialogueEvent?.Invoke("Another day.");
        Debug.Log("End finished");


    }
}
