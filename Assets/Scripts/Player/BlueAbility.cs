using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlueAbility : MonoBehaviour
{

    public Transform firePoint;
    public GameObject blueBullet;
    public GameObject purpleBullet;
    public LineRenderer aimLineRenderer;
    
    private PlayerController playerController;
    [SerializeField]
    private InputActionReference pointerPosition;
    private Vector2 pointerInput;
    private LayerMask surfacesMask;

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
                    shoot();
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
        }
    }

    private void shoot()
    {
        Instantiate(blueBullet, firePoint.position, firePoint.rotation);
    }
    private void shootPurple()
    {
        Instantiate(purpleBullet, firePoint.position, firePoint.rotation);
    }
    private void aim()
    {
        Vector3 toOther = (pointerInput - (Vector2)transform.position);
        float angle = (float)((Mathf.Atan2(toOther.x, toOther.y) / Mathf.PI) * 180f);
        if (angle < 0) angle += 360f;
        firePoint.rotation = Quaternion.Euler(0, 0, 90 - angle);
        updateLaser();
    }
    private void updateLaser()
    {
        Vector2 direction = pointerInput - (Vector2)firePoint.position;
        Vector2 laserEnd = pointerInput + direction.normalized * 10f;

        aimLineRenderer.SetPosition(0, firePoint.position);
        aimLineRenderer.SetPosition(1, laserEnd);

        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.normalized.magnitude*10f, surfacesMask);
        if (hit)
        {
            aimLineRenderer.SetPosition(1, hit.point);
        }

    }
    private Vector2 getPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}


