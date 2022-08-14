using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    Vector2 magnetPosition;

    private void Awake() {
        magnetPosition = new Vector2(transform.position.x , transform.position.y);
    }
    private void OnTriggerStay2D(Collider2D collider)        
    {
        
        if(collider.GetComponent<Collider2D>().tag == "Player"){      
            Rigidbody2D prb = collider.attachedRigidbody;


            Vector2 direction = (magnetPosition - prb.position).normalized;
            prb.velocity = new Vector2(prb.velocity.x + (direction.x), prb.velocity.y + (direction.y));
        }
    }

}
