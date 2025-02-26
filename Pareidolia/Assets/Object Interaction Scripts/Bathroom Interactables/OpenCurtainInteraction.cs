using System;
using UnityEngine;

public class OpenCurtainInteraction : ObjectInteraction
{
    [SerializeField] private GameObject _closedCurtain;
    [SerializeField] private GameObject _openCurtain;
    public static event Action CloseCurtainEvent;
    public override void interact(GameObject objectInHand)
    {
        CloseCurtain();
    }

    private void CloseCurtain()
    {
        _openCurtain.SetActive(false);
        _closedCurtain.SetActive(true);
        CloseCurtainEvent?.Invoke();
    }

    void OnEnable()
    {
        TubInteraction.GetIntoTubEvent += CloseCurtain;
    }

    void OnDisable()
    {
        TubInteraction.GetIntoTubEvent -= CloseCurtain;
    }
}
