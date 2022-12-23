using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCounter : MonoBehaviour
{
    public int deaths = 0;

    private void Start()
    {
        Debug.Log(deaths);
        DontDestroyOnLoad(gameObject);
    }

    public void addDeath()
    {
        deaths++;
    }
}
