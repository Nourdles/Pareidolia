using UnityEngine;

public class CoffeeFillController : MonoBehaviour
{
    [SerializeField] private GameObject coffeeContent; // Assign "Coffee Content" in Inspector

    void Start()
    {
        if (coffeeContent != null)
        {
            coffeeContent.SetActive(false); // Hide coffee at start
        }
    }

    void OnEnable()
    {
        KeurigInteraction.CoffeeMadeEvent += ShowCoffee;
    }

    void OnDisable()
    {
        KeurigInteraction.CoffeeMadeEvent -= ShowCoffee;
    }

    private void ShowCoffee()
    {
        if (coffeeContent != null)
        {
            coffeeContent.SetActive(true); // Make coffee visible
        }
    }
}
