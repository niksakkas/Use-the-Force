using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float respawnTimer;
    [SerializeField] private float scaleIncreaseIncrement = 0.1f;
    [SerializeField] private float respawnStartingScale = 0.4f;

    public ChargeState playerState = ChargeState.Red;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject explosionPrefab;
    public Transform respawnPortalTransform;
    
    Rigidbody2D rb;
    float playerStartingScale;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStartingScale = transform.localScale.x;
    }
    private void Update()
    {
        if (Input.GetButtonDown("ChangeCharge"))
        {
            changePlayerCharge();
        }
        else if (Input.GetButtonDown("MainAbility"))
        {
            if (playerState == ChargeState.Red)
            {
                blast();
            }
        }
        //if the player just spawned from the portal, increase his scale back to normal
        if (transform.localScale.x < playerStartingScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleIncreaseIncrement, transform.localScale.y + scaleIncreaseIncrement, transform.localScale.y + scaleIncreaseIncrement);
        }
    }
    private void changePlayerCharge()
    {
        // set player color
        if (playerState == ChargeState.Red)
        {
            playerState = ChargeState.Blue;
            spriteRenderer.material.SetFloat("_Red", 0);
            spriteRenderer.material.SetFloat("_Blue", 1);
        }
        else
        {
            playerState = ChargeState.Red;
            spriteRenderer.material.SetFloat("_Red", 1);
            spriteRenderer.material.SetFloat("_Blue", 0);
        }
    }
    //runs when player dies
    public void die()
    {
        //create explosion
        createDeathParticles();
        //die and respawn
        StartCoroutine(dieAndRespawn());
    }
    private void blast()
    {
        animator.SetTrigger("Blast");

    }
    public void createDeathParticles()
    {
        GameObject newObject = Instantiate(explosionPrefab, transform.position, transform.rotation) as GameObject;
        if (playerState == ChargeState.Red)
        {
            newObject.GetComponent<explosionController>().color = ChargeState.Red;
        }
        else if (playerState == ChargeState.Blue)
        {
            newObject.GetComponent<explosionController>().color = ChargeState.Blue;
        }
    }


    IEnumerator dieAndRespawn()
    {
        //Die
        Color color = spriteRenderer.color;
        transform.position = respawnPortalTransform.position;
        spriteRenderer.color = new Color(0f, 0f, 0f, 0f);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //Wait
        yield return new WaitForSeconds(respawnTimer);
        //Make the player smaller
        transform.localScale = new Vector3(respawnStartingScale, respawnStartingScale, respawnStartingScale);
        //Respawn
        spriteRenderer.color = color;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
