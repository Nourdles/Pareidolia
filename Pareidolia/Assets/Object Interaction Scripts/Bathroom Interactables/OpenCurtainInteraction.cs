using System;
using UnityEngine;
using FMODUnity;

public class OpenCurtainInteraction : ObjectInteraction
{
    public static event Action CloseCurtainEvent;
    [SerializeField] EventReference showerCurtainSfx;
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
