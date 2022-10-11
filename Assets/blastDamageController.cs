using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blastDamageController : MonoBehaviour
{
    public ParticleSystem part;


    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();


    void OnParticleCollision(GameObject other)
    {
        part.GetCollisionEvents(other, collisionEvents);
    }
}
