using System;
using UnityEngine;

public class ClosedCurtainInteraction : ObjectInteraction
{
    [SerializeField] private GameObject _closedCurtain;
    [SerializeField] private GameObject _openCurtain;
    public static event Action OpenCurtainEvent;
    public override void interact(GameObject objectInHand)
    {
        OpenCurtain();
    }

    private void OpenCurtain()
    {
        _closedCurtain.SetActive(false);
        _openCurtain.SetActive(true);
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
