using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityFieldController : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        disableVerticalControl(collider.gameObject);
        applyPlayerGravityPull(collider.attachedRigidbody);
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        enableVerticalControl(collider.gameObject);
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            removePlayerGravityPull(collider.attachedRigidbody);
        }
    }
    
    private void removePlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.GetComponent<Rigidbody2D>().gravityScale = 0; // remove gravity pull
    }
    private void applyPlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.GetComponent<Rigidbody2D>().gravityScale = 1; // apply gravity pull
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
