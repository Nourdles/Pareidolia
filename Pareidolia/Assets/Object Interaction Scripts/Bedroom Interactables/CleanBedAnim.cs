using Unity.VisualScripting;
using UnityEngine;

public class CleanBedAnim : MonoBehaviour
{
    [SerializeField] private GameObject CleanBedPrefab;
    private void OnEnable() {
        BedInteraction.BedInteractionEvent += changeBed;
    }

    private void OnDisable() {
        BedInteraction.BedInteractionEvent -= changeBed;
    }

    protected void changeBed()
    {
        Instantiate(CleanBedPrefab, transform.position, transform.rotation);
        Destroy(gameObject);

    }
}
