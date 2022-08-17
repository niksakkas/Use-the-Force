using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{

    public float pushMultiplier;       // when magnet and target are both blue or red
    public float hardPullMultiplier;   // when magnet and target are opposite colors
    public float softPullMultiplier;   // when either the magnet or the target are purple
    [Range(0, 30)][SerializeField] public float magnetSength;         // (0,inf) Power of the magnet
    public ChargeState magnetCharge;


    public float CalculatePullOrPush(ChargeState targetCharge)
    {
        // Debug.Log("targetCharge: " + targetCharge);
        // Debug.Log("magnetCharge: " + magnetCharge);
        switch ((int)targetCharge * (int)magnetCharge)
        {
            case (-1):
                return hardPullMultiplier;
            case (0):
                return softPullMultiplier;
            case (1):
                return pushMultiplier;
            default:
                return 0f;
        }
    }

}
