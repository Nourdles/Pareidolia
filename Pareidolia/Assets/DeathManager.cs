using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class DeathManager : MonoBehaviour
{

    public Image blackPanel;

    public static event Action<string> DeathEvent;

    public GameObject Player;

    public GameObject SpawnPoint;

    [SerializeField] private CharacterController cc;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blackPanel.color = new Color(0, 0, 0, 0);
    }

    IEnumerator waitNSeconds(float n)
    {
        yield return new WaitForSeconds(n);
    }

    void WaitForSecondsManual(float seconds)
    {
        float start = Time.time;
        while (Time.time < start + seconds)
        {
            // Do nothing, just wait
        }
    }

    public IEnumerator ProcessDeath()
    {
        DeathEvent?.Invoke("I feel dizzy. What's going on??");

        blackPanel.color = new Color(0, 0, 0, 255);

        yield return new WaitForSeconds(1.2f);

        cc.enabled = false;
        Player.transform.position = SpawnPoint.transform.position;

        cc.enabled = true;
        yield return new WaitForSeconds(3);
        blackPanel.color = new Color(0, 0, 0, 0);

        DeathEvent?.Invoke("What a weird dream?");
        
    }
}
