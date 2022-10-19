using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //portals
    public GameObject activeRespawnPortal;
    public GameObject[] pooledRedSplatters;
    public GameObject[] pooledBlueSplatters;
    public int amountToPool = 1000;
    public GameObject splatter;
    private int redPoolingCounter = 0;
    private int bluePoolingCounter = 0;

    PlayerController player;
    MagneticField[] magneticFields;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.respawnPortalTransform = activeRespawnPortal.transform;

        activeRespawnPortal.SendMessage("activate");

        magneticFields = GameObject.FindObjectsOfType<MagneticField>();

        pooledRedSplatters = new GameObject[amountToPool/2];
        pooledBlueSplatters = new GameObject[amountToPool/2];
        Vector3 splatterStartingPosition = new Vector3(0.0f, 0.0f, -1000.0f);

        for (int i = 0; i < amountToPool/2; i++)
        {
            pooledRedSplatters[i] = Instantiate(splatter, splatterStartingPosition, Quaternion.identity);
            pooledRedSplatters[i].SendMessage("pickColor", ChargeState.Red);
        }
        for (int i = 0; i < amountToPool/2; i++)
        {
            pooledBlueSplatters[i] = Instantiate(splatter, splatterStartingPosition, Quaternion.identity);
            pooledBlueSplatters[i].SendMessage("pickColor", ChargeState.Blue);
        }
    }

    // Change the active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        if (portal != activeRespawnPortal){
            activeRespawnPortal.SendMessage("deactivate");
            activeRespawnPortal = portal;
            activeRespawnPortal.SendMessage("activate");
        }
        player.respawnPortalTransform = activeRespawnPortal.transform;
    }
    void updateDirectionOfFields(){
        foreach(MagneticField field in magneticFields)
        {
            field.SendMessage("changeDirection");
        }
    }
    public GameObject setSplattersPosition(ChargeState color)
    {
        if (color == ChargeState.Red) { 
            redPoolingCounter += 1;
            return pooledRedSplatters[redPoolingCounter % pooledRedSplatters.Length];
        }
        else
        {
            bluePoolingCounter += 1;
            return pooledBlueSplatters[bluePoolingCounter % pooledBlueSplatters.Length];
        }
        //pooledRedSplatters[redPoolingCounter % pooledRedSplatters.Length].transform.position = position;

    }
    //void createSplatters(Vector3 newSplashosition)
    //{
    //    for(int i = 0; i < splatterPoolSize.Length/2; i++)
    //    {
    //        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0f, 359f)));
    //        GameObject splatterObject = Instantiate(splatter, newSplashosition, rotation);
    //        //tell splatter to pick its color
    //        splatterObject.SendMessage("pickColor", color);
    //    }

    //}
}
