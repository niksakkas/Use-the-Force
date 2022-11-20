using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum ChargeState
    {
        Blue = -1,
        Red = 1,
        Purple = 0
    }


public class GlobalVariables : MonoBehaviour
{
    public static Color redColor = new Color(1f, 0.0859375f, 0.0859375f, 1f);
    public static Color blueColor = new Color(0f, 0.66f, 1f, 1f);
    public static Color purpleColor = new Color(0.87f, 0f, 1f, 1f);

    public static int splatterCounter = 0;

}


