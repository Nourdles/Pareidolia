using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Exit the scene with a fade to black animation, and move to the next level
/// </summary>
public class CreditsFadeScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    // play fadeout animation only
    public void FadeOutAnim()
    {
        animator.SetTrigger("FadeOut");
    }

    // play fade out animation then exit scene
    public void FadeOutExit()
    {
        animator.SetTrigger("FadeOut");
        StartCoroutine(WaitForFadeOut());
    }

    // called by an animation event on the fade out animation once it ends
    /*
    public void NextLevel()
    {
        GameStateManager.MoveToNextLevel();
    } */



    IEnumerator WaitForFadeOut()
    {
        //Debug.Log("Waiting for animation");
        //while (animator.GetCurrentAnimatorStateInfo(0).length >= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        // once animation has finished, move to the next level
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("TitleScreenScene");
    }
}
