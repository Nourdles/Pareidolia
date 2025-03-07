using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSheetManager : MonoBehaviour
{
    public Sprite[] PlaystationUISprites;
    public Sprite[] XboxUISprites;
    public Sprite[] PcUISprites;

    // public Image 
    public int ID;

    void Start()
    {
        PlaystationUISprites = Resources.LoadAll<Sprite>("UISprites/PlaystationUI");
        XboxUISprites = Resources.LoadAll<Sprite>("UISprites/XboxUI");
        PcUISprites = Resources.LoadAll<Sprite>("UISprites/PCUI");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
