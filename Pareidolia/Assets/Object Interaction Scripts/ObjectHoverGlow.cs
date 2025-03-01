using System;
using UnityEngine;

public class ObjectHoverGlow : MonoBehaviour 
{

    [SerializeField] GameObject objectInView;
    public Material highlightMaterial;
    Material originalMaterial;
    GameObject lastHighlightedObject;
    public static event Action<GameObject> ViewingObjectEvent;

    void HighlightObject(GameObject gameObject)
    {
        // if we are looking at a new one
        if (lastHighlightedObject != gameObject)
        {
            MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                ClearHighlighted();
                originalMaterial = meshRenderer.material;
                meshRenderer.material = highlightMaterial;
                ViewingObjectEvent?.Invoke(gameObject);
                lastHighlightedObject = gameObject;
            }
        } 
    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            lastHighlightedObject.GetComponent<MeshRenderer>().material = originalMaterial;
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
            objectInView = hitObject;

            if (hitObject.CompareTag("InteractableObject"))
            {
                HighlightObject(hitObject);
            } else
            {
                ClearHighlighted();
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

    private void UpdateOrigMaterial(Material newMat)
    {
        originalMaterial = newMat;
    }

    void OnEnable()
    {
        BowlInteraction.ChangeBowlMat += UpdateOrigMaterial;
    }

    void OnDisable()
    {
        BowlInteraction.ChangeBowlMat -= UpdateOrigMaterial;
    }

}
