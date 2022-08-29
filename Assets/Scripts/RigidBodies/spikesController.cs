using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikesController : MonoBehaviour
{
    public PlayerController playerController;
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.GetComponent<Collider2D>().tag == "Player")
       {
            playerController.die();
       }
    }
}
