using UnityEngine;

public class ForceShaderLoad : MonoBehaviour
{
    void Start()
    {
        Shader blurShader = Shader.Find("Hidden/FullScreen/GaussianBlur");
        if (blurShader == null)
        {
            Debug.LogError("❌ Shader not found! Unity is not detecting Hidden/FullScreen/GaussianBlur.");
        }
        else
        {
            Debug.Log("✅ Shader successfully loaded.");
        }
    }
}
