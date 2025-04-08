using UnityEngine;

public class ReticleManager : MonoBehaviour
{
    [SerializeField] private GameObject reticle;

    private void EnableReticle()
    {
        reticle.SetActive(true);
    }

    private void DisableReticle()
    {
        reticle.SetActive(false);
    }

    void OnEnable()
    {
        DeathManager.DeathSceneEvent += DisableReticle;
        DeathManager.RespawnEvent += EnableReticle;
        SofaInteraction.TVStartEvent += DisableReticle;
        TVSceneManager.TVWatchedEvent += EnableReticle;
    }

    void OnDisable()
    {
        DeathManager.DeathSceneEvent -= DisableReticle;
        DeathManager.RespawnEvent -= EnableReticle;
        SofaInteraction.TVStartEvent -= DisableReticle;
        TVSceneManager.TVWatchedEvent -= EnableReticle;
    }
}
