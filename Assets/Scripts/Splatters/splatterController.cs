using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splatterController : MonoBehaviour
{
    public ChargeState color;
    //public Material redMaterial;
    //public Material blueMaterial;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();

    }
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
