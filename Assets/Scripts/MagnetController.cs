using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    public float helpFactor;           // [0,1] The degree to which the magnet's pull will assists the players current movement
    public float magnetSength;         // (0,inf) Power of the magnet
    public float distanceImportance;   // [0,inf) The higher the more powerful the magnet gets if the object is close
    
    private Vector2 magnetPosition;

    private void Awake() {
        magnetPosition = new Vector2(transform.position.x , transform.position.y);
    }
    private void OnTriggerStay2D(Collider2D collider)        
    {
        Debug.Log(collider.GetComponent<PlayerController>().state == ChargeState.Blue);

        if(collider.GetComponent<Collider2D>().tag == "Player"){      
            Rigidbody2D playerRB = collider.attachedRigidbody;
            AddNewVelocity(playerRB);
        }
    }
    private void AddNewVelocity(Rigidbody2D targetRB){
        // calculate distance between target and magnet
        float distance = Vector2.Distance(magnetPosition, targetRB.position);
        // calculate the direction of the new velocity and slightly adjust it based on the current's velocity direction
        Vector2 newVelocityDirection = ((magnetPosition - targetRB.position).normalized * (1-helpFactor)) + ((targetRB.velocity).normalized * helpFactor);
        // calculate the magnitude of the new velocity based on the magnetSength and the distanceImportance
        float newVelocityMagnitude = magnetSength/(float)Math.Pow(distance, distanceImportance);
        // add new velocity to current velocity
        targetRB.velocity += newVelocityDirection * newVelocityMagnitude;
    }
}