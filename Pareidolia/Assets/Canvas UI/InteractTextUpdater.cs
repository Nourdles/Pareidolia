using TMPro;
using UnityEngine;


/// <summary>
/// Updates the interact text on the canvas. Attach this script to the UI Updater object
/// </summary>
public class InteractTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactField;
    //[SerializeField] private GameObject _textboxObj;

    //
    private void DisplayInteractText(GameObject gameobject)
    {
        string txt_to_display;
        bool visible;
        if (gameobject != null)
        {
            txt_to_display = gameobject.GetComponent<ObjectInteraction>().GetInteractText();
            visible = true;
        } else
        {
            txt_to_display = "";
            visible = false;
        }
        _interactField.text = txt_to_display;
        //_textboxObj.SetActive(visible);
    }

    void OnEnable()
    {
        ObjectHoverGlow.ViewingObjectEvent += DisplayInteractText;
    }

    void OnDisable()
    {
        ObjectHoverGlow.ViewingObjectEvent += DisplayInteractText;
    }

}
