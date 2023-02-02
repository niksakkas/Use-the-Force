using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomogenousFieldController : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private Vector2 forceDirection;
    public PlayerController playerScript;
    public MagneticField magneticFieldScript;

    SpriteRenderer m_SpriteRenderer;
    float height;
    float width;

    private void Awake()
    {
        forceDirection = (pointB.position - pointA.position).normalized;
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        height = m_SpriteRenderer.bounds.size.y;
        width = m_SpriteRenderer.bounds.size.x;
        setShaderTilling();
    }
    private void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            Rigidbody2D playerRB = collider.attachedRigidbody;
            float pullOrPush = magneticFieldScript.CalculatePullOrPush(playerScript.playerState);
            playerRB.AddForce(forceDirection * magneticFieldScript.magnetStrength * pullOrPush * 10f);

        }
    }

    private void setShaderTilling(){
        // float multiplier = height/weight;
        Vector2 tilling = new Vector2(width/2,height/2);
        m_SpriteRenderer.material.SetVector("_DegreeOfTilling", tilling);
    }
}
