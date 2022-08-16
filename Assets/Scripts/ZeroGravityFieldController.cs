using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityFieldController : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            Debug.Log("eee");
            Rigidbody2D playerRB = collider.attachedRigidbody;
            removePlayerGravityPull(playerRB);
        }

    }
    private void removePlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.AddForce(- Physics.gravity * playerRB.mass);
    }
}
