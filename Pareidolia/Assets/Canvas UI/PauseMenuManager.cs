using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMainMenu;
    public GameObject pauseOptionsMenu;

    public Button pauseMainMenuFirstButton;
    public Button pauseOptionsMenuFirstButton;

    void Start()
    {
        ShowPauseMainMenu();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (pauseOptionsMenu.activeSelf)
            {
                ShowPauseMainMenu();
            }
        }
    }

    public void ShowPauseMainMenu()
    {
        pauseMainMenu.SetActive(true);
        pauseOptionsMenu.SetActive(false);

        StartCoroutine(ForceSelectButton(pauseMainMenuFirstButton));
    }

    public void ShowPauseOptionsMenu()
    {
        pauseMainMenu.SetActive(false);
        pauseOptionsMenu.SetActive(true);

        StartCoroutine(ForceSelectButton(pauseOptionsMenuFirstButton));
    }

    IEnumerator ForceSelectButton(Button button)
    {
        yield return null;

        if (button != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return null;
            EventSystem.current.SetSelectedGameObject(button.gameObject);
        }
    }
}