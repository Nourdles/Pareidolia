using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenuUI;
    public Selectable firstSlider;

    // brightness variables
    [SerializeField] private Volume postProcessingVolume;
    private LiftGammaGain gamma;

    private const float minGamma = -0.5f;
    private const float maxGamma = 1.0f;

    // sensibility variables
    [SerializeField] private Camera playerCamera;
    private MoveCamera moveCamera;
    private const float minSensitivity = 50f;
    private const float maxSensitivity = 1000f;

    [Header("Sliders")]
    public Slider brightnessSlider;
    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider ambienceSlider;
    public Slider sfxSlider;

    void Start()
    {
        if (optionsMenuUI.activeSelf)
        {
            StartCoroutine(ForceSelectSlider(firstSlider));
        }

        SetupListeners();

        if (OptionsManager.Instance != null) // store values across scenes
        {
            brightnessSlider.SetValueWithoutNotify(OptionsManager.Instance.gammaValue);
            sensitivitySlider.SetValueWithoutNotify(Mathf.InverseLerp(minSensitivity, maxSensitivity, OptionsManager.Instance.sensitivity));
        }

        // setup brightness slider
        if (postProcessingVolume != null && postProcessingVolume.profile.TryGet(out gamma))
        {
            gamma.active = true;
            gamma.gamma.overrideState = true;
            Vector4 g = gamma.gamma.value;
            float intensity = g.w;
            float normalized = Mathf.InverseLerp(minGamma, maxGamma, intensity);
            brightnessSlider.SetValueWithoutNotify(normalized);
            if (OptionsManager.Instance != null)
                OptionsManager.Instance.gammaValue = normalized;
        }
        else if (OptionsManager.Instance != null)
        {
            brightnessSlider.SetValueWithoutNotify(OptionsManager.Instance.gammaValue);
        }

        // setup sensibility slider
        if (playerCamera != null)
        {
            moveCamera = playerCamera.GetComponent<MoveCamera>();
            if (moveCamera != null)
            {
                float normalized = Mathf.InverseLerp(minSensitivity, maxSensitivity, moveCamera.mouseSens);
                sensitivitySlider.SetValueWithoutNotify(normalized);
                if (OptionsManager.Instance != null)
                    OptionsManager.Instance.sensitivity = moveCamera.mouseSens;
            }
        }
        else if (OptionsManager.Instance != null)
        {
            float normalized = Mathf.InverseLerp(minSensitivity, maxSensitivity, OptionsManager.Instance.sensitivity);
            sensitivitySlider.SetValueWithoutNotify(normalized);
        }        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Escape))
        {
            FindObjectOfType<PauseMenuManager>().ShowPauseMainMenu();
        }
    }

    private void SetupListeners()
    {
        brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
        sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
        ambienceSlider.onValueChanged.AddListener(OnAmbienceChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
    }

    private void OnBrightnessChanged(float value)
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.gammaValue = value;
        }
        
        if (gamma != null)
        {
            float gammaVal = Mathf.Lerp(minGamma, maxGamma, value);
            Vector4 gammaVector = new Vector4(gammaVal, gammaVal, gammaVal, gammaVal);
            gamma.gamma.overrideState = true;
            gamma.gamma.value = gammaVector;
        }
    }

    private void OnSensitivityChanged(float value)
    {
        if (OptionsManager.Instance != null)
        {
            OptionsManager.Instance.sensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, value);
        }

        if (moveCamera != null)
        {
            float mappedSens = Mathf.Lerp(minSensitivity, maxSensitivity, value);
            moveCamera.mouseSens = mappedSens;
        }
    }

    private void OnMasterVolumeChanged(float value)
    {
        // SET AUDIO MASTER VOLUME LOGIC HERE
        Debug.Log($"Master Volume set to {value}");
    }

    private void OnAmbienceChanged(float value)
    {
        // SET AUDIO AMBIENCE VOLUME LOGIC HERE
        Debug.Log($"Ambience set to {value}");
    }

    private void OnSFXChanged(float value)
    {
        // SET AUDIO SFX VOLUME LOGIC HERE
        Debug.Log($"SFX set to {value}");
    }

    IEnumerator ForceSelectSlider(Selectable slider)
    {
        yield return null;
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        if (slider != null)
        {
            EventSystem.current.SetSelectedGameObject(slider.gameObject);
        }
    }
}
