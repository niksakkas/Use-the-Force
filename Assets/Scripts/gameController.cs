using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    GameObject[] portals;
    public GameObject activeRespawnPortal;
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        portals = GameObject.FindGameObjectsWithTag("Portal");
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.respawnPortalTransform = activeRespawnPortal.transform;

    }

    // Adjust the Active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        activeRespawnPortal = portal;
        player.respawnPortalTransform = activeRespawnPortal.transform;
    }
}
