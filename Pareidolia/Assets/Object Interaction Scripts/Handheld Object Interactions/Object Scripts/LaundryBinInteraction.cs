using UnityEngine;

public class LaundryBinInteraction : HandheldObjectInteraction
{
    [SerializeField] private bool _isFull; // if all clothes have been collected
    private int _numclothes;
    private const int NUM_DIRTY_CLOTHES = 7; // the number of clothes needed to be picked up
    
    protected override void Start()
    {
        base.Start();
        handheld_id = Handhelds.LaundryBin;
        _numclothes = 0;
    }

    private void pickupClothes()
    {
        _numclothes += 1;
        // check if equal to num_dirty_clothes
        if (_numclothes == NUM_DIRTY_CLOTHES)
        {
            _isFull = true;
            InvokeDialoguePromptEvent("That should be the last of my dirty clothes");
        } else
        {
            InvokeDialoguePromptEvent(GetNumMissing());
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
}
