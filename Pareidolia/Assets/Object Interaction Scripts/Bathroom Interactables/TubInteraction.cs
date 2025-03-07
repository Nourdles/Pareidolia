using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TubInteraction : ObjectInteraction // or tub interaction
{
    [SerializeField] private GameObject _player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform _showerHoldTransform;
    [SerializeField] private Transform _matHoldTransform;
    [SerializeField] private GameObject _taskManagerObj;
    private TaskManager _taskManager;
    public static event Action GetIntoTubEvent;
    private bool _doneShower = false;
    private bool _insideShower = false;

    protected override void Start()
    {
        base.Start();
        _taskManager = _taskManagerObj.GetComponent<TaskManager>();
    }

    public override void interact(GameObject objectInHand)
    {
        if (_doneShower)
        {
            if (_insideShower)
            {
                cc.enabled = false;
                _player.transform.position = _matHoldTransform.transform.position;
                cc.enabled = true;
                _insideShower = false;

                // FOR PLAYTEST DEMOS ONLY
                LoadScene.LoadEndOfDemoScene();
            } else
            {
            InvokeDialoguePromptEvent("I already took a shower");
            }
        } else if (!_doneShower && _insideShower)
        {
            InvokeDialoguePromptEvent("I haven't finished my shower yet!!!");
        } else if (_taskManager.IsMorningComplete())
        {
            if (objectInHand != null)
            {
                InvokeDialoguePromptEvent("I can't go in the shower with this");
            } else // not holding anything
            {
                SetUninteractable();
                GetIntoTubEvent?.Invoke();
                cc.enabled = false;
                _player.transform.position = _showerHoldTransform.transform.position;
                cc.enabled = true;
                _insideShower = true;
            }
        } else 
        {
            InvokeDialoguePromptEvent("I should finish the rest of my chores before I shower");    
        }
    }


    private void FinishShower()
    {
        _doneShower = true;
    }
    
    void OnEnable()
    {
        ShowerTask.ShowerComplete += FinishShower;
        ClosedCurtainInteraction.OpenCurtainEvent += SetInteractable;
        OpenCurtainInteraction.CloseCurtainEvent += SetUninteractable;
        
    }

    void OnDisable()
    {
        ShowerTask.ShowerComplete -= FinishShower;
        ClosedCurtainInteraction.OpenCurtainEvent -= SetInteractable;
        OpenCurtainInteraction.CloseCurtainEvent -= SetUninteractable;
    }

}
