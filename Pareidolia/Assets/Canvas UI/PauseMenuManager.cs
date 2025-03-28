using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pauseMainMenu;
    public GameObject pauseOptionsMenu;

    public Button pauseMainMenuFirstButton;     // resume button
    public Button pauseOptionsMenuFirstButton;  // back or first options button

    void Start()
    {
        // start on pause main menu and auto-select Resume
        ShowPauseMainMenu();
    }

    void Update()
    {
        // "B" button (xbox) / "circle" (playstation) to go back
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

        ResetAllButtonColors(pauseMainMenu);
        StartCoroutine(ForceSelectButton(pauseMainMenuFirstButton));
    }

    public void ShowPauseOptionsMenu()
    {
        pauseMainMenu.SetActive(false);
        pauseOptionsMenu.SetActive(true);

        ResetAllButtonColors(pauseOptionsMenu);
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

    void ResetAllButtonColors(GameObject menu)
    {
        TextColorChanger[] textChangers = menu.GetComponentsInChildren<TextColorChanger>();
        foreach (TextColorChanger changer in textChangers)
        {
            changer.ResetTextColor();
        }
    }
}
