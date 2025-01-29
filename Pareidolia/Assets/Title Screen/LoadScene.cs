using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
   public void StartGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
