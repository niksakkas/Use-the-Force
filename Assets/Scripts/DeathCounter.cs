using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public int deaths = 0;
    public TMP_Text deathCounterText;
    public TMP_Text tempDeathCounterText;
    private bool toogle = true;

    private void Start()
    {
        tempDeathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.text = "Deaths: 0";
        tempDeathCounterText.text = "Deaths: 0";

        DontDestroyOnLoad(gameObject);
    }
    public void addDeath()
    {
        deaths++;


        if (toogle == true)
        {
            tempDeathCounterText.text = "Deaths: " + deaths;
            deathCounterText.CrossFadeAlpha(0f, 1f, false);
            tempDeathCounterText.CrossFadeAlpha(1f, 1f, false);
            toogle = false;
        }
        else
        {
            deathCounterText.text = "Deaths: " + deaths;
            deathCounterText.CrossFadeAlpha(1f, 1f, false);
            tempDeathCounterText.CrossFadeAlpha(0f, 1f, false);
            toogle = true;
        }
    }
}
