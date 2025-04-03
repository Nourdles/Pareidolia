using System;
using TMPro;
using UnityEngine;


/// <summary>
/// Updates the interact text on the canvas. Attach this script to the UI Updater object
/// </summary>
public class InteractTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _interactField;
    private void DisplayInteractText(GameObject gameobject)
    {
        string txt_to_display;
        if (gameobject != null)
        {
            txt_to_display = gameobject.GetComponent<ObjectInteraction>().GetInteractText();
        } else
        {
            txt_to_display = "";
        }
        _interactField.text = txt_to_display;
    }

    void OnEnable()
    {
        ObjectHoverGlow.ViewingObjectEvent += DisplayInteractText;
    }

    void OnDisable()
    {
        ObjectHoverGlow.ViewingObjectEvent -= DisplayInteractText;
    }

}
