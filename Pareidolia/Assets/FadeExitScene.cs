using UnityEngine;
using System.Collections;

/// <summary>
/// Exit the scene with a fade to black animation, and move to the next level
/// </summary>
public class FadeExitScene : MonoBehaviour
{
    [SerializeField] Animator animator;

    // Play the fade out, which will lead to a level change
    public void FadeOutExit()
    {  
        Debug.Log("fading out");
        animator.SetTrigger("FadeOut");
    }

    // called by an animation event on the fade out animation once it ends
    public void NextLevel()
    {
        GameStateManager.MoveToNextLevel();
    }


    /*
    // play fade out animation, but also cmove to the next level
    public void FadeOutExitScene()
    {
        FadeOutAnim();
        StartCoroutine(WaitForFadeOut());
    }


    IEnumerator WaitForFadeOut()
    {
        //Debug.Log("Waiting for animation");
        while (animator.GetCurrentAnimatorStateInfo(0).length + 0.5f >= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            yield return null;
        }
        // once animation has finished, move to the next level
        GameStateManager.MoveToNextLevel();
    } */
}
