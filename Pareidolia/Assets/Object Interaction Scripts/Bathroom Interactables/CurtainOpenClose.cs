using UnityEngine;
using FMODUnity;

public class CurtainOpenClose : MonoBehaviour
{
    [SerializeField] private GameObject _closedCurtain;
    [SerializeField] private GameObject _openCurtain;
    [SerializeField] EventReference showerCurtainSfx;

    private void CloseCurtain()
    {
        _closedCurtain.SetActive(true);
        _openCurtain.SetActive(false);
        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);
    }

    private void OpenCurtain()
    {
        _closedCurtain.SetActive(false);
        _openCurtain.SetActive(true);
        AudioManager.instance.PlayOneShot(showerCurtainSfx, this.transform.position);
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
