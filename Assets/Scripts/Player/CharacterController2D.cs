using UnityEngine;
using System.Collections;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	// [SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[Range(0,1)] [SerializeField] private float airResistance;
	public bool verticalControl = false;
	public float flyingSpeedMultiplier = 2.5f;
	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded = false;            // Whether or not the player is grounded.
	public bool aboutToLeaveGround = false;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
	public Animator animator;
	public Material dashIconMaterial;

	//dash
	public bool canDash = false;

    //sound

    [SerializeField] private AudioSource jumpAudioSource;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite the project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			//check if grounded
			if (colliders[i].gameObject != gameObject && colliders[i].gameObject.tag != "MagnetCollider" && colliders[i].gameObject.tag != "ZeroGravityField" && colliders[i].gameObject.tag != "HomogenousField")
			{
				m_Grounded = true;
				canDash = true;
				aboutToLeaveGround = false;
				dashIconMaterial.SetInt("_CanDash", 1);
				animator.SetBool("OnAir", false);
				return;
			}
		}
        if (!aboutToLeaveGround)
        {
			StartCoroutine(LeaveGroundSoon());
        }
		animator.SetBool("OnAir", true);

	}
	// Even if the player left the ground, allow jumping for an extra of 0.1 sec (makes the mechanic more forgiving)
	public IEnumerator LeaveGroundSoon()
    {
		aboutToLeaveGround = true;
		yield return new WaitForSeconds(0.1f);
		m_Grounded = false;

	}

	public void Move(float horizontalMove, float verticalMove, bool jump)
	{
		// If the player has no vertival control ignore vertical commands
		if (verticalControl == false)
		{
			verticalMove = 0f;
		}
		// If the player has vertival control, he is flying, so we want hom to go faster
		else
		{
			horizontalMove *= flyingSpeedMultiplier;
			verticalMove *= flyingSpeedMultiplier;
		}
		// Normal speed if player is grounded
		if (m_Grounded){
			this.applyMovement(horizontalMove, verticalMove);
		}
		// Air control is allowed, but at lower speed 
		else
		{
			this.applyMovement(horizontalMove * 0.5f, verticalMove * 0.5f);
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			jumpAudioSource.Play();
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
		applyAirResistance();
	}

	private void applyMovement(float horizontalMove, float verticalMove)
	{

		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(horizontalMove, m_Rigidbody2D.velocity.y + verticalMove);
		// And then smoothing it out and applying it to the character
		m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);
		animator.SetFloat("Xspeed", Mathf.Abs(m_Rigidbody2D.velocity.x));

		// If the input is moving the player right and the player is facing left...
		if (horizontalMove > 0 && !m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (horizontalMove < 0 && m_FacingRight)
		{
			// ... flip the player.
			Flip();
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		transform.Rotate(0, 180f, 0);
	}
	private void applyAirResistance(){
		Vector2 direction = - m_Rigidbody2D.velocity.normalized;
		float v = m_Rigidbody2D.velocity.magnitude;
		float forceAmount = v*v*airResistance;
		m_Rigidbody2D.AddForce(direction * forceAmount);
	}


}