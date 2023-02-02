using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MagnetController : MonoBehaviour
{
    [Range(0,1)][SerializeField] private float helpFactor;             // [0,1] The degree to which the magnet's pull will assists the players current movement
    [Range(0,2)][SerializeField] private float distanceImportance;    // [0,inf) The higher this is, the more powerful the magnet gets if the object is close
    [SerializeField] private PlayerController playerScript;
    [SerializeField] private MagneticField magneticFieldScript;
    public ChargeState magnetCharge = ChargeState.Blue;
    private Vector2 magnetPosition;

    //Light
    public UnityEngine.Rendering.Universal.Light2D portalLight;
    float basicLightInnerRadius = 0;
    float basicLightOuterRadius = 0;

    private void Awake() {
        magnetPosition = new Vector2(transform.position.x , transform.position.y);
        portalLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        basicLightInnerRadius = portalLight.pointLightInnerRadius;
        basicLightOuterRadius = portalLight.pointLightOuterRadius;
    }
    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
    }

    private void Update()
    {
        // pingpong magnet's light radius
        portalLight.pointLightInnerRadius = basicLightInnerRadius * (1 + Mathf.PingPong(Time.fixedTime * 0.2f, 0.2f));
        portalLight.pointLightOuterRadius = basicLightOuterRadius * (1 + Mathf.PingPong(Time.fixedTime * 0.2f, 0.2f));
    }

    //pushes player accordingly when he is within the field
    private void OnTriggerStay2D(Collider2D collider)        
    {
        if (collider.GetComponent<Collider2D>().tag == "Player"){      
            Rigidbody2D playerRB = collider.attachedRigidbody;
            float pullOrPush = magneticFieldScript.CalculatePullOrPush(playerScript.playerState);
            AddNewVelocity(playerRB, pullOrPush);
        }
    }
    private void AddNewVelocity(Rigidbody2D targetRB, float force){
        // calculate distance between target and magnet
        float distance = Vector2.Distance(magnetPosition, targetRB.position);
        // calculate the direction of the new velocity and slightly adjust it based on the current's velocity direction
        Vector2 newVelocityDirection = ((magnetPosition - targetRB.position).normalized * (1-helpFactor)) + ((targetRB.velocity).normalized * helpFactor);
        // calculate the magnitude of the new velocity based on the magnetStrength and the distanceImportance
        float newVelocityMagnitude = force * magneticFieldScript.magnetStrength / ((float)Math.Pow(distance, distanceImportance));
        // add new velocity to current velocity
        targetRB.velocity += newVelocityDirection * newVelocityMagnitude;
    }
}