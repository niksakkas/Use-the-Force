using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomogenousFieldController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Vector2 forceDirection;
    public PlayerController playerScript;
    public MagneticField magneticFieldScript;

    private void Awake()
    {
        forceDirection = (pointB.position - pointA.position).normalized;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            Rigidbody2D playerRB = collider.attachedRigidbody;
            float pullOrPush = magneticFieldScript.CalculatePullOrPush(playerScript.playerState);
            playerRB.AddForce(forceDirection * magneticFieldScript.magnetSength * pullOrPush);

        }
    }
}
