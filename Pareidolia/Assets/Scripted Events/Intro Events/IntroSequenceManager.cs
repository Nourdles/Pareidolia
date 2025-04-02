using UnityEngine;
using System;
public class IntroSequenceManager : MonoBehaviour
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
        fadeOutCanvas.FadeOutExit();
        //DreamDialogueEvent?.Invoke("Another day.");
        Debug.Log("Intro finished");


    }
}
