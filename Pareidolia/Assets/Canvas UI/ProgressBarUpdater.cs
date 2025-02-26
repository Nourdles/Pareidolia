using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUpdater : MonoBehaviour
{
    [SerializeField] private Scrollbar _progressBar;
    [SerializeField] private GameObject _handle;
    private Image _handleIMG;
    private Image _barIMG;
    private bool _isVisible = false;

    void Start()
    {
        _handleIMG = _handle.GetComponent<Image>();
        _barIMG = _progressBar.GetComponent<Image>();
    }
    private void UpdateProgressBar(float _chargeValue)
    {
        _progressBar.size = _chargeValue;
    }

    private void UpdatePBVisiblity(bool visible)
    {
        _isVisible = visible;
    }

    void Update()
    {
        _handleIMG.enabled = _isVisible;
        _barIMG.enabled = _isVisible;
        Debug.Log("Progress Bar visible: " + _isVisible);
    }

    void OnEnable()
    {
        ProgressTask.UpdateProgressBarEvent += UpdateProgressBar;
        ProgressTask.UpdatePBVisibilityEvent += UpdatePBVisiblity;
    }

    void OnDisable()
    {
        ProgressTask.UpdateProgressBarEvent -= UpdateProgressBar;
        ProgressTask.UpdatePBVisibilityEvent += UpdatePBVisiblity;
    }
}
