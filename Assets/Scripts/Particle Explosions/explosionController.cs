using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosionController : MonoBehaviour
{
    public ChargeState color;
    public ParticleSystem part;
    public GameObject splatter;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
        switch (color)
        {
            case ChargeState.Red:
                settings.startColor = new ParticleSystem.MinMaxGradient(GlobalVariables.redColor);
                break;
            case ChargeState.Blue:
                settings.startColor = new ParticleSystem.MinMaxGradient(GlobalVariables.blueColor);
                break;
            default:
                break;
        }
    }
    void OnParticleCollision(GameObject other)
    {

        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            Debug.Log(collisionEvents[i].intersection);
            Instantiate(splatter, collisionEvents[i].intersection, transform.rotation);
        }
    }
}
