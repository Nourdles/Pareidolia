using System;
using UnityEngine;
using System.Collections.Generic;

public class ObjectHoverGlow : MonoBehaviour 
{
    [SerializeField] private GameObject objectInView;
    public Material highlightMaterial; // Shader Graph-based outline
    private List<(MeshRenderer renderer, Material[] originalMaterials)> highlightedRenderers = new();
    private GameObject lastHighlightedObject;

    public static event Action<GameObject> ViewingObjectEvent;

    void HighlightObject(GameObject gameObject)
    {
        if (lastHighlightedObject == gameObject)
            return;

        ClearHighlighted();
        lastHighlightedObject = gameObject;
        objectInView = gameObject;
        ViewingObjectEvent?.Invoke(gameObject);

        highlightedRenderers.Clear();

        TryHighlightRenderer(gameObject);

        // check immediate children only
        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag("InteractChild"))
                TryHighlightRenderer(child.gameObject);
        }
    }

    void TryHighlightRenderer(GameObject obj)
    {
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
        if (meshRenderer == null) return;

        Material[] original = meshRenderer.materials;
        Material[] newMaterials = new Material[original.Length + 1];
        original.CopyTo(newMaterials, 0);
        newMaterials[^1] = highlightMaterial;

        meshRenderer.materials = newMaterials;
        highlightedRenderers.Add((meshRenderer, original));
    }


    void ClearHighlighted()
    {
        foreach (var (renderer, original) in highlightedRenderers)
        {
            if (renderer != null)
                renderer.materials = original;
        }

        highlightedRenderers.Clear();
        lastHighlightedObject = null;
        objectInView = null;
        ViewingObjectEvent?.Invoke(null);
    }

    void HighlightObjectInCenterOfCam()
    {
        float rayDistance = 5.0f;
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

    private void UpdateOrigMaterial(Material newMat)
    {
        if (lastHighlightedObject == null) return;

        for (int i = 0; i < highlightedRenderers.Count; i++)
        {
            var (renderer, originalMats) = highlightedRenderers[i];
            if (renderer == null) continue;

            if (renderer.materials.Length > 1 && renderer.materials[^1] == highlightMaterial)
            {
                for (int j = 0; j < originalMats.Length; j++)
                {
                    originalMats[j] = newMat;
                }

                Material[] updated = new Material[originalMats.Length + 1];
                originalMats.CopyTo(updated, 0);
                updated[^1] = highlightMaterial;
                renderer.materials = updated;

                // update the stored original materials in the list
                highlightedRenderers[i] = (renderer, originalMats);
            }
            else
            {
                Material[] newOriginal = new Material[] { newMat };
                renderer.materials = newOriginal;
                highlightedRenderers[i] = (renderer, newOriginal);
            }
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
