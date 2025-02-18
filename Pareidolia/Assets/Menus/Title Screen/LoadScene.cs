using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logic for the "play" button: load first scene of the game
/// </summary>

public class LoadScene : MonoBehaviour
{
   public void StartGame()
    {
        SceneManager.LoadSceneAsync("MainScene");
    }
}
