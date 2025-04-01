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
    [SerializeField] private Material windowColor;
    [SerializeField] private GameObject notepad;
    [SerializeField] private Material notepadDeteriorationMat;
    

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
        
        // change window color
        Color emissionColor = new Color(1f, 0f, 0f);
        float intensity = 1.1f;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (r.sharedMaterial == windowColor) // affect all windows
            {
                r.GetPropertyBlock(block);
                block.SetColor("_EmissionColor", emissionColor * intensity);
                r.SetPropertyBlock(block);
            }
        }

        // notepad
        DeteriorateNotepad();
    }

    private void DeteriorateNotepad(){

        if (notepad != null && notepadDeteriorationMat != null)
        {
            Renderer notepadRenderer = notepad.GetComponent<Renderer>();
            if (notepadRenderer != null)
            {
                notepadRenderer.material = notepadDeteriorationMat;
            }
        }
    }

    // after shower event
    private void DeteriorateBasement()
    {
        basementDecals.SetActive(true);
    }




}
