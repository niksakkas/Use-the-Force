using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class splatterController : MonoBehaviour
{
    public ChargeState color;
    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        switch (color)
        {
            case ChargeState.Red:
                renderer.color = new Color(GlobalVariables.redColor.r, GlobalVariables.redColor.b, GlobalVariables.redColor.g, GlobalVariables.redColor.a);
                break;
            case ChargeState.Blue:
                renderer.color = new Color(GlobalVariables.blueColor.r, GlobalVariables.blueColor.b, GlobalVariables.blueColor.g, GlobalVariables.blueColor.a);
                break;
            default:
                break;
        }
    }
}
