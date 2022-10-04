using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlueAbility : MonoBehaviour
{

    public Transform firePoint;
    public GameObject blueBullet;
    public GameObject purpleBullet;

    private PlayerController playerController;

    [SerializeField]
    private InputActionReference pointerPosition;
    Vector2 pointerInput;

    //public Vector2 PointerPosition { get; set; }
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {  
        pointerInput = getPointerInput();
        if (playerController.playerState == ChargeState.Blue || playerController.purplePowerupActive == true)
        {
            aim();
            if (Input.GetButtonDown("MainAbility"))
            {

                if ( playerController.purplePowerupActive == false)
                {
                    shoot();
                }
                else
                {
                    shootPurple();
                }
            }
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
    }
    private Vector2 getPointerInput()
    {
        Vector3 mousePos = pointerPosition.action.ReadValue<Vector2>();
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
}


