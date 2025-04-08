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
        StopAllCoroutines();
        EventSystem.current.SetSelectedGameObject(null);

        pauseOptionsMenu.SetActive(false);
        pauseMainMenu.SetActive(true);

        StartCoroutine(ForceSelectButton(pauseMainMenuFirstButton));
    }

    public void ShowPauseOptionsMenu()
    {
        StopAllCoroutines();
        EventSystem.current.SetSelectedGameObject(null);

        pauseMainMenu.SetActive(false);
        pauseOptionsMenu.SetActive(true);

        StartCoroutine(ForceSelectButton(pauseOptionsMenuFirstButton));
    }

    IEnumerator ForceSelectButton(Button targetButton)
    {
        yield return null; // Let layout settle

        GameObject activeMenu = pauseMainMenu.activeSelf ? pauseMainMenu : pauseOptionsMenu;
        Button[] allButtons = activeMenu.GetComponentsInChildren<Button>(false);

        foreach (var btn in allButtons)
        {
            if (btn != targetButton)
            {
                EventSystem.current.SetSelectedGameObject(btn.gameObject);
                yield return null;
            }
        }

        EventSystem.current.SetSelectedGameObject(null);
        yield return null;

        if (targetButton != null)
        {
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);
        }
    }
}