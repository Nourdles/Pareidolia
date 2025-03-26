using System;
using UnityEngine;
using UnityEngine.InputSystem;


/// <summary>
/// Logic for pickup up laundry bin and adding clothes. Attach to bin gameobject
/// </summary>
public class LaundryBinInteraction : HandheldObjectInteraction
{
    [SerializeField] private bool _isFull; // if all clothes have been collected
    private int _numclothes;
    private const int NUM_DIRTY_CLOTHES = 4; // the number of clothes needed to be picked up
    public static event Action PickupBinEvent;

    [SerializeField] private FMODUnity.EventReference clothingPickupSFX;

    [SerializeField] GameObject[] basketShirts;
    
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.LaundryBin;
        _numclothes = 0;
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up laundry bin";
    }

    private void pickupClothes()
    {
        _numclothes += 1;

        // play pickup sfx
        AudioManager.instance.PlayOneShot(clothingPickupSFX, transform.position);

        if (basketShirts[_numclothes - 1] != null)
        {
            basketShirts[_numclothes - 1].SetActive(true);
        }
        
        // check if equal to num_dirty_clothes
        if (_numclothes == NUM_DIRTY_CLOTHES)
        {
            _isFull = true;
            InvokeDialoguePromptEvent("That should be the last of my dirty clothes");
        } else
        {
            InvokeDialoguePromptEvent(GetNumMissing() + " clothes picked up");
        }
    }

    public void HideBasketShirts() // empty the basket when the shirts are put in the washer
    {
        Debug.Log("DoLaundryEvent received: hiding basket shirts.");

        foreach (GameObject shirt in basketShirts)
        {
            if (shirt != null)
                shirt.SetActive(false);
        }
    }

    void OnEnable()
    {
        ClothingInteraction.ClothingPickUpEvent += pickupClothes;
    }

    void OnDisable()
    {
        ClothingInteraction.ClothingPickUpEvent -= pickupClothes;
    }
    
    public bool GetIsFull()
    {
        return _isFull;
    }

    public string GetNumMissing()
    {
        return _numclothes + "/" + NUM_DIRTY_CLOTHES;
    }

    protected override void InvokePickupEvent()
    {
        PickupBinEvent?.Invoke();
    }

    protected override void UpdateInteractText()
    {
        interactText = "Press <sprite=\"UISprites\" name=\"" + 
            interactKey.GetBindingDisplayString(InputBinding.MaskByGroup(inputMasking)) + "\"> to pick up laundry bin";
    }
}
