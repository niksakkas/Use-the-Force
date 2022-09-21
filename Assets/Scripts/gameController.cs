using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //portals
    public GameObject activeRespawnPortal;

    PlayerController player;
    MagneticField[] magneticFields;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.respawnPortalTransform = activeRespawnPortal.transform;

        activeRespawnPortal.SendMessage("activate");

        magneticFields = GameObject.FindObjectsOfType<MagneticField>();
    }

    // Change the active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        if(portal != activeRespawnPortal){
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
}
