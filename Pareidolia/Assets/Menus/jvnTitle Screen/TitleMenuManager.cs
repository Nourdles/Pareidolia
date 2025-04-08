using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject creditsMenu;

    public Button mainMenuFirstButton;
    public Button optionsMenuFirstButton;
    public Button creditsMenuFirstButton;

    void Start()
    {
        // MainMenu is active at start and selects first button (PlayButton)
        ShowMainMenu();
    }

    void Update()
    {
        // "B" button (xbox) / "circle" (playstation) to go back
        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            if (optionsMenu.activeSelf)
            {
                ShowMainMenu();
            }
            else if (creditsMenu.activeSelf)
            {
                ShowMainMenu();
            }
        }
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        creditsMenu.SetActive(false);

        // reset all button text colors
        ResetAllButtonColors(mainMenu);

        // first button is always selected
        StartCoroutine(ForceSelectButton(mainMenuFirstButton));
    }

    public void ShowOptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        creditsMenu.SetActive(false);

        // reset all button text colors
        ResetAllButtonColors(optionsMenu);

        // first button is always selected
        StartCoroutine(ForceSelectButton(optionsMenuFirstButton));
    }

    public void ShowCreditsMenu()
    {
        MainMenu.StopTitleMusic();
        SceneManager.LoadScene("Credits");
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

    public void ShwoQuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit(); 
        #endif
    }

    // have to do this cus the fuckass buttons KEEP GETTING STUCK AS SELECTED HOLY SHIT MAN
    void ResetAllButtonColors(GameObject menu)
    {
        TextColorChanger[] textChangers = menu.GetComponentsInChildren<TextColorChanger>();
        foreach (TextColorChanger changer in textChangers)
        {
            changer.ResetTextColor();
        }
    }
}
