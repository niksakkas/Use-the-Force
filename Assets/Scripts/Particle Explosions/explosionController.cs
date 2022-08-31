using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionController : MonoBehaviour
{
    public ChargeState color;
    void Start()
    {
        ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
        switch (color)
        {
            case ChargeState.Red:
                settings.startColor = new ParticleSystem.MinMaxGradient(GlobalVariables.redColor);
                break;
            case ChargeState.Blue:
                settings.startColor = new ParticleSystem.MinMaxGradient(GlobalVariables.blueColor);
                break;
            default:
                break;
        }
    }
}
