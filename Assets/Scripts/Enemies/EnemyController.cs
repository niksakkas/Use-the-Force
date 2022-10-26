using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public AIPath aiPath;
    public GameObject player;
    public float minDistance;

    private float distance;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Debug.Log(distance);
        if(distance > minDistance)
        {
            gameObject.GetComponent<AIDestinationSetter>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<AIDestinationSetter>().enabled = true;
        }
        
            if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}
