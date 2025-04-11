using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class SofaInteraction : ObjectInteraction
{
    public static event Action TVStartEvent; // notify that the tv task has been started
    protected override void Start()
    {
        base.Start();
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
        task = taskManager.GetComponentInChildren<TVTask>();
        TVTask.CrossOutTaskEvent += EnableStainDamage;
    }
    protected override void interactaction(GameObject objectInHand)
    {
        SanityTracker.damageEnabled = false; //Turning off stain damage until scene done
        SetUninteractable();
        TVStartEvent?.Invoke();
        StartCoroutine(ResetAndLoadTVScene());
    }

    private void EnableStainDamage(int _)
    {

        SanityTracker.damageEnabled = true;
    }

    private System.Collections.IEnumerator ResetAndLoadTVScene()
    {
        yield return null;
        yield return null;
        // Unload the scene if it's already loaded (fix for Unity caching old additive scene)
        if (UnityEngine.SceneManagement.SceneManager.GetSceneByName("TVWatch").isLoaded)
        {
            SceneSwitcher.UnLoadSceneOnTop("TVWatch");

            // Wait until it's fully unloaded before continuing
            while (UnityEngine.SceneManagement.SceneManager.GetSceneByName("TVWatch").isLoaded)
            {
                yield return null;
            }
        }

        // Now load and activate the scene
        SceneSwitcher.LoadSceneOnTop("TVWatch");

        // Wait a frame or two to allow proper load
        yield return null;
        yield return null;

        SceneSwitcher.SetSceneActive("TVWatch");
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to watch TV";
    }
}
