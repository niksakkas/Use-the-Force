using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityFieldController : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        disableVerticalControl(collider.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        enableVerticalControl(collider.gameObject);
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            Rigidbody2D playerRB = collider.attachedRigidbody;
            removePlayerGravityPull(playerRB);
        }
    }

    private void removePlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.AddForce(-Physics.gravity * playerRB.mass);
    }
    private void enableVerticalControl(GameObject player)
    {
        player.GetComponent<CharacterController2D>().verticalControl = true;
    }
    private void disableVerticalControl(GameObject player)
    {
        player.GetComponent<CharacterController2D>().verticalControl = false;
    }
}
