using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeathCounter : MonoBehaviour
{
    public int deaths = 0;
    public TMP_Text deathCounterText;

    private void Start()
    {
        deathCounterText.text = "Deaths: 0";
    
        DontDestroyOnLoad(gameObject);
    }
    public void addDeath()
    {
        deaths++;
        deathCounterText.text = "Deaths: " + deaths;
    }
}
