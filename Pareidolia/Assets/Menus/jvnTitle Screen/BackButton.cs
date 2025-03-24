using UnityEngine;
using UnityEngine.UI;

public class BackButtonUI : MonoBehaviour
{
    public GameObject currentMenu;
    public GameObject previousMenu;

    private Button backButton;

    void Start()
    {
        // get button component attached to this gameObject
        backButton = GetComponent<Button>();

        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBack);
        }
    }

    void Update()
    {
        // if this button's menu is active and the player presses "B" (xbox) / "circle" (playstation) on the controller
        if (currentMenu.activeSelf && Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            GoBack();
        }
    }

    public void GoBack()
    {
        // hide current menu
        if (currentMenu != null) 
            currentMenu.SetActive(false);

        // show previous menu
        if (previousMenu != null) 
            previousMenu.SetActive(true);
    }
}
