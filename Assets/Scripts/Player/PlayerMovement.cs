using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;
	public Animator animator;
	public float disabledTimer = 0f;
	public float runSpeed = 400f;
	float horizontalMove = 0f;
	float verticalMove = 0f;
	bool jump = false;
	void Update () {

        if (disabledTimer == 0)
        {
			horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
			verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;
			animator.SetBool("IsMoving", horizontalMove != 0f);

			if (Input.GetButtonDown("Jump"))
			{
				jump = true;
			}
		}
	}
	void FixedUpdate ()
	{
		if (disabledTimer == 0)
		{
			// Move the player
			controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, jump);
			jump = false;
		}
        else
        {
			disabledTimer--;
        }
	}
}