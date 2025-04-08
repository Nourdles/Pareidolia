using UnityEngine;
using System.Collections;

public class CoffeeFillController : MonoBehaviour
{
    [SerializeField] private GameObject coffeeContent; // assign "Coffee Content" in Inspector

    void Start()
    {
        if (coffeeContent != null)
        {
            coffeeContent.SetActive(false); // hide coffee at start
        }
    }

    void OnEnable()
    {
        KeurigInteraction.CoffeeMadeEvent += DelayedShowCoffee;
    }

    void OnDisable()
    {
        KeurigInteraction.CoffeeMadeEvent -= DelayedShowCoffee;
    }

    private void DelayedShowCoffee()
    {
        StartCoroutine(ShowCoffeeAfterDelay(1f)); // wait 1 second
    }

    private IEnumerator ShowCoffeeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (coffeeContent != null)
        {
            coffeeContent.SetActive(true); // make coffee visible after delay
        }
    }
}
