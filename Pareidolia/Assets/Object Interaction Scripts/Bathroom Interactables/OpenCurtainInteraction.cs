using System;
using UnityEngine;
using FMODUnity;
using UnityEngine.InputSystem;

public class OpenCurtainInteraction : ObjectInteraction
{
    public static event Action CloseCurtainEvent;
    [SerializeField] EventReference showerCurtainSfx;

    protected override void Start()
    {
        base.Start();
        interactText = "Press " + interactKey.GetBindingDisplayString() + " to close curtain";
    }
    public override void interact(GameObject objectInHand)
    {
        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);
        InvokeCloseCurtain();
    }

    private void InvokeCloseCurtain()
    {
        CloseCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += InvokeCloseCurtain;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= InvokeCloseCurtain;
    }
}
