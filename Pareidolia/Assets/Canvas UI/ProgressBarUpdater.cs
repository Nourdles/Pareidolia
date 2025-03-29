using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUpdater : MonoBehaviour
{
    [SerializeField] private Image _progressFill;
    [SerializeField] private Image _progressBase;
    [SerializeField] private GameObject _handle;

    private Image _handleIMG;
    private bool _isVisible = false;

    void Start()
    {
        if (_handle != null)
            _handleIMG = _handle.GetComponent<Image>();
    }

    private void UpdateProgressBar(float chargeValue)
    {
        chargeValue = Mathf.Clamp01(chargeValue);
        _progressFill.fillAmount = chargeValue;
    }

    private void UpdatePBVisibility(bool visible)
    {
        _isVisible = visible;
    }

    void Update()
    {
        if (_handleIMG != null)
            _handleIMG.enabled = _isVisible;

        if (_progressBase != null)
            _progressBase.enabled = _isVisible;

        if (_progressFill != null)
            _progressFill.enabled = _isVisible;
    }

    void OnEnable()
    {
        ProgressTask.UpdateProgressBarEvent += UpdateProgressBar;
        ProgressTask.UpdatePBVisibilityEvent += UpdatePBVisibility;
    }

    void OnDisable()
    {
        ProgressTask.UpdateProgressBarEvent -= UpdateProgressBar;
        ProgressTask.UpdatePBVisibilityEvent -= UpdatePBVisibility;
    }
}