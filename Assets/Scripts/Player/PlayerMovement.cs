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

	public float dashDistance = 10f;
	int dashRaycastMask;
	public Rigidbody2D rb;


    private void Awake()
    {
		rb = GetComponent<Rigidbody2D>();
		dashRaycastMask = (1 << LayerMask.NameToLayer("Surfaces") | (1 << LayerMask.NameToLayer("Enemies")));

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
        if (Input.GetButtonDown("Dash"))
        {
			Vector2 dir = getDashDirection();
			Vector2 target = dir;
			Vector2 origin = transform.position;
			rb.AddForce(dir * dashDistance);
		}
    }
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
			return new Vector2(Mathf.Sign(horizontalMove) * Mathf.Sqrt(0.5f), Mathf.Sign(verticalMove) * Mathf.Sqrt(0.5f));
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
				x = Mathf.Sign(horizontalMove);

			}
			if (verticalMove == 0f)
			{
				y = 0;
			}
			else
			{
				y = Mathf.Sign(verticalMove);

			}
			return new Vector2(x, y);
            
		}
	}
}