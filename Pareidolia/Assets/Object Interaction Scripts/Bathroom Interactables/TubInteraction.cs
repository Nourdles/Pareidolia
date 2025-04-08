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
    public static event Action GetIntoTubEvent;
    private bool _doneShower = false;
    private bool _insideShower = false;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step into the tub";
        //task = taskManager.GetComponentInChildren<ShowerTask>();
    }

    protected override void interactaction(GameObject objectInHand)
    {
        if (_doneShower)
        {
            if (_insideShower)
            {
                cc.enabled = false;
                _player.transform.position = _matHoldTransform.transform.position;
                cc.enabled = true;
                _insideShower = false;
            } else
            {
            InvokeDialoguePromptEvent("I already took a shower");
            }
        } else if (!_doneShower && _insideShower)
        {
            InvokeDialoguePromptEvent("I haven't finished my shower yet!!!");
        } else // not done shower and not inside shower
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
