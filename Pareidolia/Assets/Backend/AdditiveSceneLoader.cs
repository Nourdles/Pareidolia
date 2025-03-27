using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public static class SceneSwitcher
{
    public static event Action AddingSceneEvent;
    public static event Action RemovingSceneEvent;
    public static void LoadSceneOnTop(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
        AddingSceneEvent?.Invoke();
    }

    public static void UnLoadSceneOnTop(string scene)
    {
        int n = SceneManager.sceneCount;
        if (n > 1)
        {
            Debug.Log("Unloading " + scene);
            SceneManager.UnloadSceneAsync(scene);
            RemovingSceneEvent?.Invoke();
        }
    }
}
