using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splatterController : MonoBehaviour
{
    public ChargeState color;


    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

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
