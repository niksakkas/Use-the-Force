using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
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
        // foreach(Object field in GameObject.FindObjectsOfType<MagneticField>()) //des an iparxei method tou object
        // {
        //     magneticFields.append(field.GetComponent<MagneticField>());
        // }
        updateDirectionOfFields();
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
