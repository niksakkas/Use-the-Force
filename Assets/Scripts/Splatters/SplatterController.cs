using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatterController : MonoBehaviour
{
    public Sprite[] splatterSprites;
    public Color minRed;
    public Color maxRed;
    public Color minBlue;
    public Color maxBlue;

    private float minBH, minBS, minBV, minRH, minRS, minRV;
    private float maxBH, maxBS, maxBV, maxRH, maxRS, maxRV;

    void Awake()
    {
        Color.RGBToHSV(minBlue, out minBH, out minBS, out minBV);
        Color.RGBToHSV(maxBlue, out maxBH, out maxBS, out maxBV);
        Color.RGBToHSV(minRed, out minRH, out minRS, out minRV);
        Color.RGBToHSV(maxRed, out maxRH, out maxRS, out maxRV);


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
                //Debug.Log(minRH + "," + maxRH + "," + minRS + "," + maxRS + "," + minRV + "," + maxRV);a
                renderer.color = Random.ColorHSV(minRH, maxRH, minRS, maxRS, minRV, maxRV);
                //Debug.Log(renderer.color);
                //Debug.Log(Random.ColorHSV(0f, 0.038f, 0.82f, 0.90f, 1f, 1f));


                break;
            case ChargeState.Blue:
                renderer.color = Random.ColorHSV(minBH, maxBH, minBS, maxBS, minBV, maxBV);
                break;
            default:
                break;
        }
    }
}
