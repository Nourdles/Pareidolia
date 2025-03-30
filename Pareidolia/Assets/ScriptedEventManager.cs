using System;
using Unity.VisualScripting;
using UnityEngine;

public class ScriptedEventManager : MonoBehaviour
{
    private int numScriptedEventMorn = 3;
    private TaskManager taskManager;

    // morning events
    //[SerializeField] BasementDoorScriptedEvent basementDoorEvent;
    //[SerializeField] SilhouetteFlickerEvent flickerEvent;
    [SerializeField] TVFaceEvent TVFaceEvent;

    //[SerializeField] SofaInteraction sofaInteraction;

    [SerializeField] GameObject upperFloorDecals;
    [SerializeField] GameObject basementDecals;

    //[SerializeField] UpdateUI notepadUI;

    private bool eventTriggered = false;


    
    // Update is called once per frame
    void Update()
    {
        
    }


    /*void Start()
    {
        taskManager = UnityEngine.Object.FindFirstObjectByType<TaskManager>();

        basementDoorInteraction = GetComponent<DoorInteraction>();

        if (basementDoorInteraction == null)
        {
            Debug.LogError("ScriptedDoorEvent: No DoorInteraction found on this object.");
        }

        basementDecals.SetActive(false);
        upperFloorDecals.SetActive(false);

        Task.CompleteTaskEvent += OnTaskCompleted;

    }*/

    private void OnEnable()
    {
        //SilhouetteFlickerEvent.EventStart -= BasementEventStart;
        //SilhouetteFlickerEvent.EventEnd -= BasementEventEnd;

        SofaInteraction.TVStartEvent += StartTVFaceEvent;
        SofaInteraction.TVStartEvent += DeteriorateUpperFloor;

        ShowerTask.ShowerComplete += DeteriorateBasement;
    }

    private void OnDestroy()
    {
        //Task.CompleteTaskEvent -= OnTaskCompleted;
        SofaInteraction.TVStartEvent -= StartTVFaceEvent;
        SofaInteraction.TVStartEvent -= DeteriorateUpperFloor;

        ShowerTask.ShowerComplete -= DeteriorateBasement;


    }

    /*private void OnTaskCompleted()
    {
        if (taskManager.IsMorningComplete())
        {
            basementDoorEvent.StartEvent();
        }
    }*/

    // once player starts tv watch task, spawn in faces
    private void StartTVFaceEvent()
    {
        TVFaceEvent.StartEvent();
    }

    // after tv event
    private void DeteriorateUpperFloor()
    {
        upperFloorDecals.SetActive(true);
    }

    // after shower event
    private void DeteriorateBasement()
    {
        basementDecals.SetActive(true);
    }




}
