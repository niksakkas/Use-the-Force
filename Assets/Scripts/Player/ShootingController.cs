using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShootingController : MonoBehaviour
{

    public Transform firePoint;
    public GameObject blueBullet;
    public GameObject purpleBullet;
    public LineRenderer aimLineRenderer;
    public Color blueAimLineColor;
    public Color purpleAimLineColor;
    public Color baseColor;
    public Rigidbody2D m_rigidbody2D;


    public GameObject blueHit;
    public GameObject purpleHit;

    private Animator playerAnimator;
    private Throw throwScript;
    private PlayerMovement playerMovement;
    private PlayerController playerController;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput;
    private LayerMask surfacesMask;
    private GameObject blueHitEmissionGameObject;
    private GameObject purpleHitEmissionGameObject;
    private Quaternion rotation;



    private void Awake()
    {
        blueHitEmissionGameObject = Instantiate(blueHit);
        blueHitEmissionGameObject.SetActive(false);
        purpleHitEmissionGameObject = Instantiate(purpleHit);
        purpleHitEmissionGameObject.SetActive(false);
        playerController = GetComponent<PlayerController>();
        playerMovement = GetComponent<PlayerMovement>();
        throwScript = GetComponent<Throw>();
        playerAnimator = GetComponent<Animator>();
        aimLineRenderer.material.SetColor("_Color", baseColor * 3);
    }
    void Start()
    {
        surfacesMask = LayerMask.GetMask("Surfaces");
    }

    // Update is called once per frame
    void Update()
    {  
        pointerInput = getPointerInput();
        //only aim if the player is blue or purple, and is standing still
        if ((playerController.playerState == ChargeState.Blue || playerController.purplePowerupActive == true) && m_rigidbody2D.velocity.magnitude == 0f)
        {

            aimLineRenderer.enabled = true;
            aim();
            if (Input.GetButtonDown("MainAbility"))
            {
                throwAnimation();
            }
        }
        else
        {
            aimLineRenderer.enabled = false;
            blueHitEmissionGameObject.SetActive(false);
            purpleHitEmissionGameObject.SetActive(false);
        }
    }

    private void shootBlue()
    {
        GameObject newBlueBullet = Instantiate(blueBullet, firePoint.position, firePoint.rotation);
    }
    private void shootPurple()
    {
        Instantiate(purpleBullet, firePoint.position, firePoint.rotation);
    }
    private void aim()
    {
        if (playerController.purplePowerupActive == false)
        {
            //aimLineRenderer.material.SetColor("_Color", blueAimLineColor *3 );
            aimLineRenderer.material.SetInt("_PurplePowerup", 0);
        }
        else
        {
            aimLineRenderer.material.SetInt("_PurplePowerup", 1);
            //aimLineRenderer.material.SetColor("_Color", purpleAimLineColor );

        }
        Vector3 toOther = (pointerInput - (Vector2)transform.position);
        float angle = (float)((Mathf.Atan2(toOther.x, toOther.y) / Mathf.PI) * 180f);
        if (angle < 0) angle += 360f;
        rotation = Quaternion.Euler(0, 0, 90 - angle);
        firePoint.rotation = rotation;
        updateLaser();
    }

    public void shoot()
    {

        //disable player for a short duration
        playerMovement.disabledTimer = 15f;

        if (playerController.purplePowerupActive == false)
        {
            playerController.updatePurplePower(0.1f);
            shootBlue();
        }
        else
        {
            shootPurple();
        }
    }
    private void updateLaser()
    {
        Vector2 direction = pointerInput - (Vector2)firePoint.position;
        Vector2 laserEnd = pointerInput + direction.normalized * 20f;

        aimLineRenderer.SetPosition(0, firePoint.position);
        aimLineRenderer.SetPosition(1, laserEnd);

        handleRaycastHit(direction);

    }
    private Vector2 getPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void handleRaycastHit(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.normalized.magnitude * 20f, surfacesMask);
        if (hit)
        {
            aimLineRenderer.SetPosition(1, hit.point);
            if (playerController.purplePowerupActive == false)
            {
                updateHitEmission(hit.point, blueHitEmissionGameObject, purpleHitEmissionGameObject);
            }
            else
            {
                updateHitEmission(hit.point, purpleHitEmissionGameObject, blueHitEmissionGameObject);
            }
        }
        else
        {
            blueHitEmissionGameObject.SetActive(false);
            purpleHitEmissionGameObject.SetActive(false);
        }
    }
    private void updateHitEmission(Vector2 hitPos, GameObject activeEmissionObject, GameObject disabledEmissionObject)
    {

        activeEmissionObject.transform.position = hitPos;
        activeEmissionObject.SetActive(true);
        disabledEmissionObject.SetActive(false);
    }
    public void disableAiming()
    {
        purpleHitEmissionGameObject.SetActive(false);
        blueHitEmissionGameObject.SetActive(false);
    }
    private void throwAnimation()
    {
        if (throwScript.enabled == false)
        {
            playerAnimator.enabled = false;
            throwScript.enabled = true;
            throwScript.floatIndexAnim = 0;
        }

    }

}
