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
            Vector2 direction = (magnetPosition - collider.attachedRigidbody.position).normalized;
            collider.attachedRigidbody.velocity = new Vector2(direction.x, direction.y ) * 3f;
        }
    }

}
