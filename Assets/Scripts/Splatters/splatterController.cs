using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splatterController : MonoBehaviour
{
    public Sprite[] splatterSprites;
    void Start()
    {
        //pick a random splatter sprite
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Sprite randomSprite = splatterSprites[Random.Range(0, splatterSprites.Length)];
        renderer.sprite = randomSprite;
        //put it in front of previous splatters
        renderer.sortingOrder = GlobalVariables.splatterCounter;
        GlobalVariables.splatterCounter++;
    }
    //pick splatter color
    void pickColor(ChargeState charge)
    {
        
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (charge)
        {
            case ChargeState.Red:
                renderer.color = GlobalVariables.redColor;
                break;
            case ChargeState.Blue:
                renderer.color = GlobalVariables.blueColor;
                break;
            default:
                break;
        }
    }
}
