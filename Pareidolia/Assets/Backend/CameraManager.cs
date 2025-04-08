using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject mainCamera;
    private OpenCloseNote noteOpener;
    private MoveCamera cameraMover;
    private MovePlayer playerMover;

    void Start()
    {
        noteOpener = mainCamera.GetComponentInChildren<OpenCloseNote>();
        cameraMover = mainCamera.GetComponentInChildren<MoveCamera>();
        playerMover = mainCamera.GetComponentInChildren<MovePlayer>();
    }

    private void DeactivateMainCamera()
    {
        noteOpener.enabled = false;
        cameraMover.enabled = false;
        playerMover.enabled = false;
    }

    private void ActivateMainCamera()
    {
        noteOpener.enabled = true;
        cameraMover.enabled = true;
        playerMover.enabled = true;
    }

    void OnEnable()
    {
        SceneSwitcher.AddingSceneEvent += DeactivateMainCamera;
        SceneSwitcher.RemovingSceneEvent += ActivateMainCamera;
    }

    void OnDisable()
    {
        SceneSwitcher.AddingSceneEvent -= DeactivateMainCamera;
        SceneSwitcher.RemovingSceneEvent -= ActivateMainCamera;
    }
}
