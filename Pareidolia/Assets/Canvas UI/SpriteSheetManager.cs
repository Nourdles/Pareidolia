using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager : MonoBehaviour
{
    public Sprite[] PcUISprites;

    public int ID;

    void Start()
    {
        PcUISprites = Resources.LoadAll<Sprite>("UISprites");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
