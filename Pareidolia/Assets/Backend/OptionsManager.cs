using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using FMODUnity;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager Instance;
    [Range(0f, 1f)] public float gammaValue = 0.5f; // Normalized value
    public float sensitivity = 500f;

    private MoveCamera moveCamera;
    private LiftGammaGain gamma;

    private const float minGamma = -0.5f;
    private const float maxGamma = 1.0f;

    [Range(0f, 1f)] public float masterVolume = -1f;
    [Range(0f, 1f)] public float ambienceVolume = -1f;
    [Range(0f, 1f)] public float sfxVolume = -1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryApplySettings(); // when new scene is loaded, apply values again
    }

    private void TryApplySettings()
    {
        var mainCam = Camera.main;
        if (mainCam != null && mainCam.TryGetComponent(out MoveCamera mc))
        {
            moveCamera = mc;
            moveCamera.mouseSens = sensitivity;
        }

        var volumeObj = GameObject.FindWithTag("PostProcessing");
        if (volumeObj != null && volumeObj.TryGetComponent(out Volume vol))
        {
            if (vol.profile.TryGet(out LiftGammaGain g))
            {
                gamma = g;
                gamma.gamma.overrideState = true;
                float mappedGamma = Mathf.Lerp(minGamma, maxGamma, gammaValue);
                gamma.gamma.value = new Vector4(mappedGamma, mappedGamma, mappedGamma, mappedGamma);
            }
        }

        // fallback to default volume if not initialized
        if (masterVolume < 0f) masterVolume = 1f;
        if (ambienceVolume < 0f) ambienceVolume = 1f;
        if (sfxVolume < 0f) sfxVolume = 1f;

        RuntimeManager.GetBus("bus:/").setVolume(masterVolume);
        RuntimeManager.GetBus("bus:/Game/Ambience").setVolume(ambienceVolume);
        RuntimeManager.GetBus("bus:/Game/SFX").setVolume(sfxVolume);
    }

    // Manual update if needed
    public void ApplySettingsNow()
    {
        TryApplySettings();
    }
}
