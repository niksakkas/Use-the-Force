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

            createSplatter(collisionEvents[0].intersection, collisionEvents[0].velocity.magnitude);

    }
    void createSplatter(Vector3 newSplashosition, float velocityMagnitude)
    {


        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0f, 360f)));
        GameObject newObject = Instantiate(splatter, newSplashosition, rotation);
        float scaleMultiplier = Random.Range(0.05f, 0.1f) * velocityMagnitude;

        newObject.GetComponent<Transform>().localScale *= scaleMultiplier;

        if (color == ChargeState.Red)
        {
            newObject.GetComponent<splatterController>().color = ChargeState.Red;
        }
        else if (color == ChargeState.Blue)
        {
            newObject.GetComponent<splatterController>().color = ChargeState.Blue;
        }
    }
}
