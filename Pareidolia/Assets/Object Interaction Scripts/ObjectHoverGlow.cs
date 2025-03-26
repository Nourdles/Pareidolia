using System;
using UnityEngine;

public class ObjectHoverGlow : MonoBehaviour 
{
    [SerializeField] private GameObject objectInView;
    public Material highlightMaterial; // Shader Graph-based outline
    private Material[] originalMaterials;
    private GameObject lastHighlightedObject;

    public static event Action<GameObject> ViewingObjectEvent;

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject == gameObject)
            return;

        ClearHighlighted();

        MeshRenderer meshRenderer = gameObject.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer != null)
        {
            // save original materials
            originalMaterials = meshRenderer.materials;

            // append the outline material
            Material[] newMaterials = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(newMaterials, 0);
            newMaterials[^1] = highlightMaterial;

            meshRenderer.materials = newMaterials;

            lastHighlightedObject = gameObject;
            objectInView = gameObject;
            ViewingObjectEvent?.Invoke(gameObject);
        }
    }

    void ClearHighlighted()
    {
        if (lastHighlightedObject != null)
        {
            MeshRenderer meshRenderer = lastHighlightedObject.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null && originalMaterials != null)
                meshRenderer.materials = originalMaterials;

            lastHighlightedObject = null;
            objectInView = null;
        }

        ViewingObjectEvent?.Invoke(null);
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 7.0f;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            GameObject hitObject = hit.collider.gameObject;
            objectInView = hitObject;

            if (hitObject.CompareTag("InteractableObject"))
            {
                HighlightObject(hitObject);
            }
            else
            {
                ClearHighlighted();
            }
        }
        else
        {
            ClearHighlighted();
        }
    }

    void Update()
    {
        HighlightObjectInCenterOfCam();
    }

    private void UpdateOrigMaterial(Material newMat) // messy as hell but makes it compatible with highlighting
    {
        if (lastHighlightedObject == null) return;

        MeshRenderer mr = lastHighlightedObject.GetComponentInChildren<MeshRenderer>();
        if (mr == null) return;

        // check if outline is currently applied
        if (mr.materials.Length > 1 && mr.materials[^1] == highlightMaterial)
        {
            // update the base materials (exclude outline)
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                originalMaterials[i] = newMat;
            }

            // replace materials array in renderer
            Material[] updated = new Material[originalMaterials.Length + 1];
            originalMaterials.CopyTo(updated, 0);
            updated[^1] = highlightMaterial;
            mr.materials = updated;
        }
        else
        {
            // update for objects without outline (still need to store new mat)
            originalMaterials = new Material[] { newMat };
            mr.materials = originalMaterials;
        }
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
