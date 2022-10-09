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
    public GameObject blueHit;
    public GameObject purpleHit;

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
    }
    void Start()
    {
        surfacesMask = LayerMask.GetMask("Surfaces");
        playerController = GetComponent<PlayerController>();

    }

    // Update is called once per frame
    void Update()
    {  
        pointerInput = getPointerInput();
        if (playerController.playerState == ChargeState.Blue || playerController.purplePowerupActive == true)
        {
            aimLineRenderer.enabled = true;
            aim();
            if (Input.GetButtonDown("MainAbility"))
            {

                if ( playerController.purplePowerupActive == false)
                {
                    playerController.updatePurplePower(0.1f);
                    shootBlue();
                }
                else
                {
                    shootPurple();
                }
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
        blueHit.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        Instantiate(blueBullet, firePoint.position, firePoint.rotation);
    }
    private void shootPurple()
    {
        Instantiate(purpleBullet, firePoint.position, firePoint.rotation);
    }
    private void aim()
    {
        if (playerController.purplePowerupActive == false)
        {
            aimLineRenderer.material.SetColor("_Color", blueAimLineColor);
        }
        else
        {
            aimLineRenderer.material.SetColor("_Color", purpleAimLineColor);

        }
        Vector3 toOther = (pointerInput - (Vector2)transform.position);
        float angle = (float)((Mathf.Atan2(toOther.x, toOther.y) / Mathf.PI) * 180f);
        if (angle < 0) angle += 360f;
        rotation = Quaternion.Euler(0, 0, 90 - angle);
        firePoint.rotation = rotation;
        updateLaser();
    }
    private void updateLaser()
    {
        Vector2 direction = pointerInput - (Vector2)firePoint.position;
        Vector2 laserEnd = pointerInput + direction.normalized * 10f;

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
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.normalized.magnitude * 10f, surfacesMask);
        if (hit)
        {
            aimLineRenderer.SetPosition(1, hit.point);
            if (playerController.purplePowerupActive == false)
            {
                updateHitEmission(hit.point, direction, blueHitEmissionGameObject, purpleHitEmissionGameObject);
            }
            else
            {
                updateHitEmission(hit.point, direction, purpleHitEmissionGameObject, blueHitEmissionGameObject);
            }
        }
        else
        {
            blueHitEmissionGameObject.SetActive(false);
            purpleHitEmissionGameObject.SetActive(false);
        }
    }
    private void updateHitEmission(Vector2 hitPos, Vector2 direction, GameObject activeEmissionObject, GameObject disabledEmissionObject)
    {

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.Euler(angle, -90, 0f);
        activeEmissionObject.transform.rotation = q;
        activeEmissionObject.transform.position = hitPos;
        activeEmissionObject.SetActive(true);
        disabledEmissionObject.SetActive(false);
    }
}
