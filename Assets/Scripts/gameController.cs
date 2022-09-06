using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    //portals
    public GameObject activeRespawnPortal;
    public Material activePortalMaterial;
    public Material inactivePortalMaterial;

    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player.respawnPortalTransform = activeRespawnPortal.transform;

    }

    // Adjust the Active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        activeRespawnPortal.GetComponent<SpriteRenderer>().material = inactivePortalMaterial;
        activeRespawnPortal = portal;
        activeRespawnPortal.GetComponent<SpriteRenderer>().material = activePortalMaterial;
        player.respawnPortalTransform = activeRespawnPortal.transform;
    }
}
