using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Logic for the "play" button: load first scene of the game
/// </summary>

public class LoadScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    // current scene fades out (called before startGame)
    public void FadeToLevel()
    {
        animator.SetTrigger("FadeOut");
    }

    // once title scene fades out, load the next scene
    public void LoadNextScene()
    {
        //FadeToLevel();
        SceneManager.LoadSceneAsync("MainScene");
    }


}
