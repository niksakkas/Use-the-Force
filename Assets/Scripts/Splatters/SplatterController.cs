using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
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
                renderer.color = Random.ColorHSV(0f, 0.038f, 0.82f, 0.90f, 1f, 1f);
                break;
            case ChargeState.Blue:
                renderer.color = Random.ColorHSV(0.52f, 0.60f, 0.87f, 0.87f, 0.87f, 0.87f);
                break;
            default:
                break;
        }
    }
}
