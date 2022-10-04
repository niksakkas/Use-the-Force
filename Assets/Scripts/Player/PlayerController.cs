using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float respawnTimer;
    [SerializeField] private float scaleIncreaseIncrement = 2f;
    [SerializeField] private float respawnStartingScale = 0.4f;

    public ChargeState playerState = ChargeState.Red;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public GameObject deathExplosionPrefab;
    public GameObject redExplosionPrefab;
    public Transform respawnPortalTransform; 
    public bool purplePowerupActive = false;
    public float purplePowerUpTimer;

    private GameObject currentRedExplosion;
    private Rigidbody2D rb;
    private GameController gameController;
    private float playerStartingScale;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStartingScale = transform.localScale.x;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void Update()
    {
        if (Input.GetButtonDown("ChangeCharge") && playerState!=ChargeState.Purple)
        {
            changePlayerCharge();
        }
        if (Input.GetButtonDown("MainAbility"))
        {
            if (playerState == ChargeState.Red && purplePowerupActive == false)
            {
                blast();
            }
        }
        if (Input.GetButtonDown("PurplePowerup"))
        {
            StartCoroutine(purplePowerUp());
        }
            //if the player just spawned from the portal, increase his scale back to normal
            if (transform.localScale.x < playerStartingScale)
        {
            transform.localScale = new Vector3(transform.localScale.x + scaleIncreaseIncrement*Time.deltaTime, transform.localScale.y + scaleIncreaseIncrement*Time.deltaTime, transform.localScale.y + scaleIncreaseIncrement*Time.deltaTime);
        }
        if (currentRedExplosion)
        {
            currentRedExplosion.GetComponent<Rigidbody2D>().velocity = rb.velocity;
        }
    }
    private void changePlayerCharge()
    {
        // update the direction of all magnetic fields
        gameController.SendMessage("updateDirectionOfFields", gameObject);


        // set player color
        if (playerState == ChargeState.Red)
        {
            becomeBlue();
        }
        else
        {
            becomeRed();
        }
    }
    private void becomeBlue()
    {
        playerState = ChargeState.Blue;
        spriteRenderer.material.SetFloat("_Red", 0);
        spriteRenderer.material.SetFloat("_Blue", 1);
    }
    private void becomeRed()
    {
        playerState = ChargeState.Red;
        spriteRenderer.material.SetFloat("_Red", 1);
        spriteRenderer.material.SetFloat("_Blue", 0);
    }
    private void becomePurple()
    {
        playerState = ChargeState.Purple;
        spriteRenderer.material.SetFloat("_Red", 1);
        spriteRenderer.material.SetFloat("_Blue", 1);
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

    private void playRedExplosion()
    {
        currentRedExplosion = Instantiate(redExplosionPrefab, transform.position, transform.rotation) as GameObject;
    }
    public void createDeathParticles()
    {
        GameObject newObject = Instantiate(deathExplosionPrefab, transform.position, transform.rotation) as GameObject;
        if (playerState == ChargeState.Red)
        {
            newObject.GetComponent<ExplosionController>().color = ChargeState.Red;
        }
        else if (playerState == ChargeState.Blue)
        {
            newObject.GetComponent<ExplosionController>().color = ChargeState.Blue;
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
    IEnumerator purplePowerUp()
    {
        purplePowerupActive = true;
        yield return new WaitForSeconds(purplePowerUpTimer);
        purplePowerupActive = false;

    }
}
