using UnityEngine.SceneManagement;
using UnityEngine;

public static class SceneSwitcher
{
    public static void LoadSceneOnTop(string scene)
    {
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    public static void UnLoadSceneOnTop(string scene)
    {
        int n = SceneManager.sceneCount;
        if (n > 1)
        {
            Debug.Log("Unloading " + scene);
            SceneManager.UnloadSceneAsync(scene);
        }
    }
}
