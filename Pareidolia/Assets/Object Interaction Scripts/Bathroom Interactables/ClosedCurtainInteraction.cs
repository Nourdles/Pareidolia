using System;
using UnityEngine;
using FMODUnity;

public class ClosedCurtainInteraction : ObjectInteraction
{
    [SerializeField] EventReference showerCurtainSfx;
    public static event Action OpenCurtainEvent;
    public override void interact(GameObject objectInHand)
    {

        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);

        OpenCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        OpenCurtainInteraction.CloseCurtainEvent += SetUninteractable;
        ShowerTask.ShowerComplete += SetInteractable;
    }

    void OnDisable()
    {
        OpenCurtainInteraction.CloseCurtainEvent -= SetUninteractable;
        ShowerTask.ShowerComplete -= SetInteractable;
    }
}
