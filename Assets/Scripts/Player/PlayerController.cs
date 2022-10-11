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
    public float purplePowerUpDuration;
    public float purplePower = 0;
    public Material purplePowerUpIconMaterial;

    private GameObject currentRedExplosion;
    private Rigidbody2D rb;
    private GameController gameController;
    private float playerStartingScale;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStartingScale = transform.localScale.x;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);

    }
    private void Update()
    {
        purplePowerUpIconMaterial.SetFloat("_Fill", purplePower);

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

    //player's blast animation triggers the playRedExplosion() function
    private void playRedExplosion()
    {
        updatePurplePower(0.1f);
        currentRedExplosion = Instantiate(redExplosionPrefab, transform.position, transform.rotation) as GameObject;
        StartCoroutine(destroyRedExplosion(currentRedExplosion));
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
    public void updatePurplePower(float change)
    {
        if (purplePower < 0f || purplePower > 1f)
        {
            return;
        }
        purplePower += change;
        if (purplePower < 0f)
        {
            purplePower = 0f;
        }
        if (purplePower > 1f)
        {
            purplePower = 1f;
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
    //this coroutine depletes the purple power over "purplePowerUpDuration" seconds. Each change happens every 0.1 second.
    IEnumerator purplePowerUp()
    {
        float secondInterval = 0.1f;
        float increment = secondInterval / purplePowerUpDuration;
        if (purplePower == 1f && purplePowerupActive == false)
        {
            purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 1);
            purplePowerupActive = true;
            while (purplePower > 0)
            {
                purplePower -= increment;
                yield return new WaitForSeconds(secondInterval);
            }
            purplePower = 0;
            purplePowerupActive = false;
            purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);

        }
    }
    IEnumerator destroyRedExplosion(GameObject redExplosionObject)
    {
        Debug.Log(redExplosionObject);
        yield return new WaitForSeconds(5);

        Destroy(redExplosionObject);
    }
}
