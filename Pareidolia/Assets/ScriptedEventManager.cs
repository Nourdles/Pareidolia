using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

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

        DeteriorateWindows();
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

    private void DeteriorateWindows()
    {
        // color change
        Color emissionColor = new Color(0.749f, 0.0078f, 0f);
        float intensity = 3.138f;

        MaterialPropertyBlock block = new MaterialPropertyBlock();
        Renderer[] renderers = FindObjectsOfType<Renderer>();

        foreach (Renderer r in renderers)
        {
            if (r.sharedMaterial == windowColor)
            {
                r.GetPropertyBlock(block);
                block.SetColor("_EmissionColor", emissionColor * intensity);
                r.SetPropertyBlock(block);
            }
        }

        // enable rain
        GameObject[] allWindows = GameObject.FindGameObjectsWithTag("Window");

        foreach (GameObject window in allWindows)
        {
            Transform rainTransform = window.GetComponentsInChildren<Transform>(true)
                .FirstOrDefault(t => t.name == "Window Rain");

            if (rainTransform != null)
            {
                ParticleSystem rainSystem = rainTransform.GetComponent<ParticleSystem>();
                if (rainSystem != null)
                {
                    rainSystem.gameObject.SetActive(true);
                    rainSystem.Play();
                }
            }
        }
    }

    // after shower event
    private void DeteriorateBasement()
    {
        basementDecals.SetActive(true);
    }




}
