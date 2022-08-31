using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ChargeState playerState = ChargeState.Red;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public ParticleSystem RedDeathParticles;
    public ParticleSystem BlueDeathParticles;

    private void Update()
    {
        if (Input.GetButtonDown("ChangeCharge"))
        {
            changePlayerCharge();
        }
        if (Input.GetButtonDown("MainAbility"))
        {
            if(playerState == ChargeState.Red)
            {
                blast();
            }
        }
    }
    private void changePlayerCharge()
    {
        if(playerState == ChargeState.Red) { 
            playerState = ChargeState.Blue;
            spriteRenderer.material.SetFloat("_Red", 0);
            spriteRenderer.material.SetFloat("_Blue", 1);
        }
        else
        {
            playerState = ChargeState.Red;
            spriteRenderer.material.SetFloat("_Red", 1);
            spriteRenderer.material.SetFloat("_Blue", 0);
        }
    }
    public void die()
    {
        if (playerState == ChargeState.Red)
        {
            Instantiate(RedDeathParticles, transform.position, transform.rotation);
        }
        else if(playerState == ChargeState.Blue)
        {
            Instantiate(BlueDeathParticles, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
    private void blast()
    {
        animator.SetTrigger("Blast");

    }
}
