using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
    [SerializeField] private bool _canShower = false;

    protected override void Start()
    {
        base.Start();
        _taskManager = _taskManagerObj.GetComponent<TaskManager>();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step into the tub";
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
        } else if (_canShower)
        {
            if (objectInHand != null)
            {
                InvokeDialoguePromptEvent("I can't go in the shower with this");
            } else // not holding anything
            {
                SetUninteractable();
                GetIntoTubEvent?.Invoke();
                interactText = "Press <sprite=\"UISprites\" name=\"" + 
                    interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step out of the tub";
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

    protected override void UpdateInteractText()
    {
        if (_insideShower)
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step out of the tub";
        } else
        {
            interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step into the tub";
        }
    }

    private void FinishShower()
    {
        _doneShower = true;
    }

    private void CanShower()
    {
        _canShower = true;
    }
    
    void OnEnable()
    {
        ShowerTask.ShowerComplete += FinishShower;
        ClosedCurtainInteraction.OpenCurtainEvent += SetInteractable;
        OpenCurtainInteraction.CloseCurtainEvent += SetUninteractable;
        LaundryMachineInteraction.DoLaundryEvent += CanShower;
    }

    void OnDisable()
    {
        ShowerTask.ShowerComplete -= FinishShower;
        ClosedCurtainInteraction.OpenCurtainEvent -= SetInteractable;
        OpenCurtainInteraction.CloseCurtainEvent -= SetUninteractable;
        LaundryMachineInteraction.DoLaundryEvent -= CanShower;
    }

}
