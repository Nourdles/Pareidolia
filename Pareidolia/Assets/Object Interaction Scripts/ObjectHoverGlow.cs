using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Highlights : MonoBehaviour {

    public Material highlightMaterial;
    Material originalMaterial;
    GameObject lastHighlightedObject;

    void HighlightObject(GameObject gameObject)
    {
        // if we are looking at a new one
        if (lastHighlightedObject != gameObject)
        {
            ClearHighlighted();
            originalMaterial = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            if (gameObject.CompareTag("Interactable"))
            {
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = highlightMaterial;
                //HoveringObject?.Invoke();
            }
            //OffHoveringObject?.Invoke();
            lastHighlightedObject = gameObject;
        }

    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.GetComponent<MeshRenderer>().sharedMaterial = originalMaterial;
            lastHighlightedObject = null;
        }
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 500.0f;
        // Ray from the center of the viewport.
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit rayHit;
        
        if (Physics.Raycast(ray, out rayHit, rayDistance))
        {
            GameObject hitObject = rayHit.collider.gameObject;
            HighlightObject(hitObject);

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
