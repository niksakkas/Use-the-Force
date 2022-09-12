using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    //portals
    public GameObject activeRespawnPortal;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.respawnPortalTransform = activeRespawnPortal.transform;

    }

    // Change the active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        activeRespawnPortal.SendMessage("deactivate");
        activeRespawnPortal = portal;
        activeRespawnPortal.SendMessage("activate");

        player.respawnPortalTransform = activeRespawnPortal.transform;
    }
}
