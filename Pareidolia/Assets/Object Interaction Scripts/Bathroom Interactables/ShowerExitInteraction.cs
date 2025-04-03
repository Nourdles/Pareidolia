using UnityEngine;
using UnityEngine.InputSystem;

public class ShowerExitInteraction : ObjectInteraction // or tub interaction
{
    [SerializeField] private GameObject _player;
    [SerializeField] private CharacterController cc;
    [SerializeField] private Transform _matHoldTransform;

    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step out of the tub";
    }

    protected override void interactaction(GameObject objectInHand)
    {
        cc.enabled = false;
        _player.transform.position = _matHoldTransform.transform.position;
        cc.enabled = true;
        gameObject.SetActive(false);
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
                interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to step out of the tub";
    }
    
    void OnEnable()
    {
        ShowerTask.ShowerComplete += SetInteractable;
    }

    void OnDisable()
    {
        ShowerTask.ShowerComplete -= SetInteractable;
    }

}
