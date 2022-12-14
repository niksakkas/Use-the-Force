using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    public ChargeState color;
    public ParticleSystem part;
    public GameObject splatter;
    public GameController gameController;
    [SerializeField] private float minScale = 0.24f;
    [SerializeField] private float maxScale = 0.48f;
    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

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
        StartCoroutine(destroyExplosion());
    }
    void OnParticleCollision(GameObject other)
    {
        //create splatters on particle collisions
        part.GetCollisionEvents(other, collisionEvents);
        createSplatter(collisionEvents[0].intersection, collisionEvents[0].velocity.magnitude);
    }
    void createSplatter(Vector3 newSplashosition, float velocityMagnitude)
    {
        //get splatter from pool
        GameObject splatterObject = gameController.GetComponentInChildren<GameController>().setSplattersPosition(color);
        //set splatter position to the point of collision
        splatterObject.transform.position = newSplashosition;
        //add a random rotation and scale to splatter
        Quaternion rotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, Random.Range(0f, 359f)));
        splatterObject.transform.rotation = rotation;
        float scaleMultiplier = Random.Range(minScale, maxScale) * velocityMagnitude;
        //display splatter at the front
        splatterObject.GetComponent<Transform>().localScale = splatter.GetComponent<Transform>().localScale * scaleMultiplier;
        splatterObject.GetComponent<SpriteRenderer>().sortingOrder = GlobalVariables.splatterCounter;
        GlobalVariables.splatterCounter++;
    }
    IEnumerator destroyExplosion()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}