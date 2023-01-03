using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Gamecontroller
    private GameController gameController;

    //Main components
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private Rigidbody2D rb;

    //Player charge (colors)
    public ChargeState playerState = ChargeState.Red;
    public float swapCharge;
    public Material swapChargeIconMaterial;
    public GameObject SwapChargeCooldownIcon;
    public float chargeSwapCooldown = 1f;

    //Death and respawn
    private float playerStartingScale;
    public GameObject deathExplosionPrefab;
    public Transform respawnPortalTransform;
    [SerializeField] private float respawnTimer;
    [SerializeField] private float scaleIncreaseIncrement = 2f;
    [SerializeField] private float respawnStartingScale = 0.4f;
    DeathCounter deathCounter;

    //Purple powerup
    public float purplePower = 0f;
    public bool purplePowerupActive = false;
    public float purplePowerUpDuration;
    public Material purplePowerUpIconMaterial;
    public ParticleSystem powerUpFireBlue;
    public ParticleSystem powerUpFireRed;

    //Blast (red)
    private GameObject currentRedExplosion;
    public GameObject redExplosionPrefab;

    //Aiming stuff (blue)
    public GameObject aimingLaser;
    public ShootingController shootingController;
    
    //Shadows casting
    public GameObject shadowsParent;
    public GameObject currentShadow;

    //Light
    private UnityEngine.Rendering.Universal.Light2D playerLight;
    public Color blueLightColor;
    public Color redLightColor;



    private void Start()
    {
        deathCounter = GameObject.FindGameObjectWithTag("DeathCounter")?.GetComponent<DeathCounter>();
        playerLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        currentShadow = shadowsParent.transform.Find(spriteRenderer.sprite.name).gameObject;
        rb = gameObject.GetComponent<Rigidbody2D>();
        playerStartingScale = transform.localScale.x;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);
        powerUpFireRed.Stop();
        powerUpFireBlue.Stop();
        swapCharge = 0f;
        swapChargeIconMaterial.SetFloat("_IconCharge", 0f);
    }
    private void Update()
    {
        updatePlayerShadowsCaster();
        purplePowerUpIconMaterial.SetFloat("_Fill", purplePower);
        if (Input.GetButtonDown("ChangeCharge") && playerState!=ChargeState.Purple)
        {
            // handle swap charge cooldown
            if (Time.time >= swapCharge)
            {
                swapCharge = Time.time + chargeSwapCooldown; 
                changePlayerCharge();
            }
        }
        //if (Input.GetButtonDown("MainAbility"))
        //{
        //    //only blast if the player is red, powerup is inactive, and player is moving
        //    if (playerState == ChargeState.Red && purplePowerupActive == false && rb.velocity.magnitude != 0)
        //    {
        //        blast();
        //    }
        //}
        //if (Input.GetButtonDown("PurplePowerup"))
        //{
        //    StartCoroutine(purplePowerUp());
        //}
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
        gameController.rotateAndRecolorChargeIcon();
        playerLight.color = blueLightColor;
    }
    private void becomeRed()
    {
        playerState = ChargeState.Red;
        spriteRenderer.material.SetFloat("_Red", 1);
        spriteRenderer.material.SetFloat("_Blue", 0);
        gameController.rotateAndRecolorChargeIcon();
        playerLight.color = redLightColor;
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
            purplePowerUpIconMaterial.SetFloat("_PowerUpActive", 0);
        }
        //tell game controller to kill and respawn the player
        gameController.killPlayer(respawnTimer);
        deathCounter.addDeath();
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
        //enableAiming();
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

    public void updatePlayerShadowsCaster()
    {
        string shadowsCasterName = spriteRenderer.sprite.name;

        //throw animations are pretty much like idle
        if(shadowsCasterName == "")
        {
            shadowsCasterName = "Idle1";
        }
        shadowsCasterName = HandleRunShadows(shadowsCasterName);

        if (currentShadow != null)
        {
            Transform currentTransform = shadowsParent.transform.Find(shadowsCasterName);
            if (currentTransform != null)
            {
                GameObject newShadow = shadowsParent.transform.Find(shadowsCasterName).gameObject;
                if (currentShadow != newShadow)
                {
                    currentShadow.SetActive(false);
                    currentShadow = newShadow;
                    currentShadow.SetActive(true);
                }
            }
        }

    }

    private string HandleRunShadows(string shadowsCasterName)
    {
        string[] splitString = shadowsCasterName.Split("Run");
        //int runNumber = 0;

        if (splitString.Length != 2)
        {

            return shadowsCasterName;
        }
        else
        {
            //runNumber = int.Parse(splitString[1]);
            //if (runNumber <= 6 )
            //{
            //    runNumber = 3;
            //}
            //else
            //{
            //    runNumber = 9;
            //}
            //return ("Run" + runNumber);
            return ("Run1");
        }
    }
}
