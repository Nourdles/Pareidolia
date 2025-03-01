using UnityEngine;

public class CurtainOpenClose : MonoBehaviour
{
    [SerializeField] private GameObject _closedCurtain;
    [SerializeField] private GameObject _openCurtain;

    private void CloseCurtain()
    {
        _closedCurtain.SetActive(true);
        _openCurtain.SetActive(false);
        PlayCurtainSFX();
    }

    private void OpenCurtain()
    {
        _closedCurtain.SetActive(false);
        _openCurtain.SetActive(true);
        PlayCurtainSFX();
    }

    private void PlayCurtainSFX()
    {
        // insert code for sfx here
    }

    void OnEnable()
    {
        OpenCurtainInteraction.CloseCurtainEvent += CloseCurtain;
        ClosedCurtainInteraction.OpenCurtainEvent += OpenCurtain;
    }

    void OnDisable()
    {
        OpenCurtainInteraction.CloseCurtainEvent -= CloseCurtain;
        ClosedCurtainInteraction.OpenCurtainEvent -= OpenCurtain;
    }
}
