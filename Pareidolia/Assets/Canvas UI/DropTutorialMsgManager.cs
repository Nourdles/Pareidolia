using UnityEngine;

public class DropTutorialMsgManager : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;
    [SerializeField] private GameObject messageText;
    

    private void ActivateDropIntructions(GameObject gameObject)
    {
        messageBox.SetActive(true);
        messageText.SetActive(true);
    }

    private void DeactivateDropIntructions()
    {
        messageBox.SetActive(false);
        messageText.SetActive(false);
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        HandheldObjectInteraction.PickUpEvent += ActivateDropIntructions;
        PlayerInteract.DropItemEvent += DeactivateDropIntructions;
    }

    void OnDisable()
    {
        HandheldObjectInteraction.PickUpEvent -= ActivateDropIntructions;
        PlayerInteract.DropItemEvent -= DeactivateDropIntructions;
    }

}
