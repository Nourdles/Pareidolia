using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ShowerInteraction : ObjectInteraction // or tub interaction
{
    [SerializeField] private GameObject _player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform _showerHoldTransform;
    [SerializeField] private GameObject showerExitInteraction;
    public static event Action GetIntoTubEvent;
    private bool _doneShower = false;

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
            InvokeDialoguePromptEvent("I already took a shower");
        } else // not done shower and not inside shower
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
            }
        }
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step into the tub";
    }

    private void FinishShower()
    {
        _doneShower = true;
        showerExitInteraction.SetActive(true);
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
