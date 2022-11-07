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
    public float purplePower = 0f;
    public Material purplePowerUpIconMaterial;
    public Material swapChargeIconMaterial;
    public float chargeSwapCooldown = 1f;
    public GameObject SwapChargeCooldownIcon;

    //powerup fires
    public ParticleSystem powerUpFireBlue;
    public ParticleSystem powerUpFireRed;
    // aiming stuff
    public GameObject aimingLaser;
    public ShootingController shootingController;

    private GameObject currentRedExplosion;
    private Rigidbody2D rb;
    private GameController gameController;
    private float playerStartingScale;
    private float swapCharge;


    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStartingScale = transform.localScale.x;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);
        powerUpFireRed.Stop();
        powerUpFireBlue.Stop();
        swapCharge = chargeSwapCooldown;
        swapChargeIconMaterial.SetFloat("_Cooldown", chargeSwapCooldown);
        swapChargeIconMaterial.SetFloat("_IconCharge", 0f);

    }
    private void Update()
    {
        swapCharge += Time.fixedDeltaTime * 0.1f;
        swapChargeIconMaterial.SetFloat("_SwapCharge", swapCharge);
        purplePowerUpIconMaterial.SetFloat("_Fill", purplePower);
        if (Input.GetButtonDown("ChangeCharge") && playerState!=ChargeState.Purple)
        {
            if (swapCharge >= chargeSwapCooldown)
            {
                swapCharge = 0;
                changePlayerCharge();
            }
        }
        if (Input.GetButtonDown("MainAbility"))
        {
            //only blast if the player is red, powerup is inactive, and 
            if (playerState == ChargeState.Red && purplePowerupActive == false && rb.velocity.magnitude != 0)
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
        swapColor();
    }
    private void swapColor()
    {
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
        //swapChargeIconMaterial.SetFloat("_IconCharge", 1f);
        StartCoroutine(rotateAndRecolorChargeIcon());

    }
    private void becomeRed()
    {
        playerState = ChargeState.Red;
        spriteRenderer.material.SetFloat("_Red", 1);
        spriteRenderer.material.SetFloat("_Blue", 0);
        //swapChargeIconMaterial.SetFloat("_IconCharge", 0f);
        StartCoroutine(rotateAndRecolorChargeIcon());

    }
    private IEnumerator rotateAndRecolorChargeIcon()
    {
        float rotation = 1.8f;
        int iterations = Mathf.CeilToInt( 180f / rotation);
        float timeSpanLength = 0.5f/iterations;
        float colorChangeIncrement = 1f / iterations;
        float colorChange = swapChargeIconMaterial.GetFloat("_IconCharge");
        if (colorChange == 1)
        {
            for (int i = 0; i < iterations; i++)
            {
                SwapChargeCooldownIcon.transform.Rotate(new Vector3(0, 0f, -rotation));
                colorChange -= colorChangeIncrement;
                swapChargeIconMaterial.SetFloat("_IconCharge", colorChange);
                yield return new WaitForSeconds(timeSpanLength);
            }
        }
        else
        {
            for (int i = 0; i < iterations; i++)
            {
                SwapChargeCooldownIcon.transform.Rotate(new Vector3(0, 0f, +rotation));
                colorChange += colorChangeIncrement;
                swapChargeIconMaterial.SetFloat("_IconCharge", colorChange);
                yield return new WaitForSeconds(timeSpanLength);
            }
        }
        swapChargeIconMaterial.SetFloat("_IconCharge", Mathf.Round(colorChange));
    }

    //runs when player dies
    public void die()
    {
        //create explosion
        createDeathParticles();
        //remove active purple powerup
        if (purplePowerupActive)
        {
            purplePowerupActive = false;
            purplePower = 0;
        }
        //tell game controller to kill and respawn the player
        gameController.killPlayer(respawnTimer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Collider2D>().tag == "Enemy")
        {
            die();
        }
        if (collision.collider.GetComponent<Collider2D>().tag == "Spikes")
        {
            die();
        }
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

    public void respawn()
    {
        powerUpFireRed.Stop();
        powerUpFireBlue.Stop();
        //Make the player smaller
        transform.position = respawnPortalTransform.position;
        transform.localScale = new Vector3(respawnStartingScale, respawnStartingScale, respawnStartingScale);
        //Respawn
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        enableAiming();
    }
    private void disableAiming()
    {
        shootingController.disableAiming();
        shootingController.enabled = false;
        aimingLaser.SetActive(false);
    }
    private void enableAiming()
    {
        shootingController.enabled = true;
        aimingLaser.SetActive(true);
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
                handlePowerUpColor();
                purplePower -= increment;
                yield return new WaitForSeconds(secondInterval);
            }
            purplePower = 0;
            purplePowerupActive = false;
            purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);
            // stop emitting fire when powerup is over
            powerUpFireRed.Stop();
            powerUpFireBlue.Stop();
        }
    }
    private void handlePowerUpColor()
    {
        if(playerState == ChargeState.Blue)
        {
            powerUpFireBlue.Play();
            powerUpFireRed.Stop();
        }
        else
        {
            powerUpFireBlue.Stop();
            powerUpFireRed.Play();
        }
    }
    IEnumerator destroyRedExplosion(GameObject redExplosionObject)
    {
        yield return new WaitForSeconds(5);
        Destroy(redExplosionObject);
    }
}
