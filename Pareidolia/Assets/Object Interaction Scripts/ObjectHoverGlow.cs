using System;
using UnityEngine;

public class PlayerView : MonoBehaviour 
{

    public Material highlightMaterial;
    Material originalMaterial;
    GameObject lastHighlightedObject;
    public static event Action<GameObject> ViewingObjectEvent;

    void HighlightObject(GameObject gameObject)
    {
        // if we are looking at a new one
        if (lastHighlightedObject != gameObject)
        {
            if (gameObject.GetComponent<MeshRenderer>() != null)
            {
                ClearHighlighted();
                originalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
                ViewingObjectEvent?.Invoke(gameObject);
                lastHighlightedObject = gameObject;
            }
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
            lastHighlightedObject = null;
            ViewingObjectEvent?.Invoke(lastHighlightedObject);
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 3.0f;
        // Ray from the center of the viewport.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;
        
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            GameObject hitObject = rayHit.collider.gameObject;
            if (hitObject.CompareTag("InteractableObject"))
            {
                HighlightObject(hitObject);
            }
        } else
        {
                ClearHighlighted();
        }
    }

    void Update()
    {
        HighlightObjectInCenterOfCam();
    }

}
