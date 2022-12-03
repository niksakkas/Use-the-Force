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
	

	// Dashing
	public float dashSpeed = 100f;
	private int dashXmultiplier = 500;
	private int dashYmultiplier = 130;
	public Rigidbody2D rb;
	public TrailRenderer playerTrail;
	PlayerController playerController;
	CharacterController2D characterController2D;


	private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		playerTrail = GetComponent<TrailRenderer>();
		playerController = GetComponent<PlayerController>();
		characterController2D = GetComponent<CharacterController2D>();
	}
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
		HandleDash();
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

	void HandleDash()
    {
        if (Input.GetButtonDown("Dash") && CanDash())
        {
			Vector2 dir = getDashDirection();
            if (dir.magnitude == 0)
            {
				dir = new Vector2(0f, Mathf.Sign(verticalMove) * dashYmultiplier);

			}
			StartCoroutine(EnableDashTrail());
			StartCoroutine(Dash(dir));
		}
	}
	private bool CanDash()
    {
		return (!characterController2D.m_Grounded && characterController2D.canDash);
    }
	IEnumerator Dash(Vector2 dir)
    {

		int dashIncrements = 20;
		float dashIncrementLength = dashSpeed/dashIncrements;
		// Apply a force smoothly over 20 increments
		// The force should lower every frame, so that the player doesn't have extra force when the dash ends
        for (int i = 0; i < dashIncrements; i++)
        {
			characterController2D.canDash = false;
			rb.AddForce(dir * dashIncrementLength * ((dashIncrements - i) /dashIncrements));
			yield return new WaitForEndOfFrame();
		}
	}
	//old dash handling
	//RaycastHit2D TryDashing(Vector2 playerOrigin, Vector2 playerTarget)
	//   {
	//	Debug.Log(playerOrigin);
	//	Debug.Log(playerTarget.normalized);
	//	RaycastHit2D hit = Physics2D.Raycast(playerOrigin, playerTarget, dashDistance, dashRaycastMask);
	//	Debug.Log(hit.distance);

	//	return hit;

	//}
	//RaycastHit2D CanDash(Vector2 origin, Vector2 target)
	//{

	//	return Physics2D.Raycast(origin, target.normalized, dashDistance, dashRaycastMask);

	//}
	Vector2 getDashDirection()
    {
		if(horizontalMove != 0f && verticalMove != 0f)
        {
            return new Vector2(Mathf.Sign(horizontalMove) * Mathf.Sqrt(0.5f) * dashXmultiplier, Mathf.Sign(verticalMove) * Mathf.Sqrt(0.5f) * dashYmultiplier);
        }

        else
        {
            float x;
			float y;
			if(horizontalMove == 0f)
            {
				x = 0;
            }
            else
            {
				x = Mathf.Sign(horizontalMove)* dashXmultiplier;

			}
			if (verticalMove == 0f)
			{
				y = 0;
			}
			else
			{
				y = Mathf.Sign(verticalMove)* dashYmultiplier;

			}
			return new Vector2(x, y);

        }
    }

	IEnumerator EnableDashTrail()
    {
		SpriteRenderer rb = GetComponent<SpriteRenderer>();
		Material playerMaterial = rb.material;
        if (playerController.playerState == ChargeState.Red)
        {
			playerTrail.material.color = GlobalVariables.redColor;
		}
		else if(playerController.playerState == ChargeState.Blue)
        {
			playerTrail.material.color = GlobalVariables.blueColor;

		}
        else
        {
			playerTrail.material.color = Color.green;

		}
		playerTrail.emitting = true;
		yield return new WaitForSeconds(0.3f);
		playerTrail.emitting = false;
	}
}