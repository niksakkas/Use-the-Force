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
                Debug.Log(GetComponent<SpriteRenderer>().color);
                break;
            case ChargeState.Blue:
                renderer.sharedMaterial.color = new Color(255, 255, 255, 255);
                break;
            default:
                break;
        }
    }
}
