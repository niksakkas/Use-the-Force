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
        ParticleSystemRenderer renderer = GetComponent<ParticleSystemRenderer>();
        //pick explosion color
        switch (color)
        {
            case ChargeState.Red:
                renderer.material.SetFloat("_Red", 1);
                renderer.material.SetFloat("_Blue", 0);
                break;
            case ChargeState.Blue:
                renderer.material.SetFloat("_Red", 0);
                renderer.material.SetFloat("_Blue", 1);
                break;
            default:
                break;
        }
    }
    void OnParticleCollision(GameObject other)
    {
        //create splatters on particle collisions
        int numCollisionEvents = part.GetCollisionEvents(other, collisionEvents);
            createSplatter(collisionEvents[0].intersection, collisionEvents[0].velocity.magnitude);

    }
    void createSplatter(Vector3 newSplashosition, float velocityMagnitude)
    {

        //add a random rotation and scale to splatter
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0f, 360f)));
        GameObject splatterObject = Instantiate(splatter, newSplashosition, rotation);
        float scaleMultiplier = Random.Range(0.12f, 0.24f) * velocityMagnitude;
        splatterObject.GetComponent<Transform>().localScale *= scaleMultiplier;
        //tell splatter to pick its color
        splatterObject.SendMessage("pickColor", color);
    }
}
