using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{

    public float pushMultiplier;       // when magnet and target are both blue or red
    public float hardPullMultiplier;   // when magnet and target are opposite colors
    public float softPullMultiplier;   // when either the magnet or the target are purple
    [Range(0, 50)][SerializeField] public float magnetStrength;   // (0,inf) Power of the magnet
    public ChargeState magnetCharge;
    SpriteRenderer m_SpriteRenderer;
    PlayerController player;
    float direction;

    // Sound
    [SerializeField] private AudioSource playerInFieldAudioSource;

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
        if (player)
        {
            m_SpriteRenderer = GetComponent<SpriteRenderer>();
            displayMagnitude();
            setShaderDirection();
            direction = CalculatePullOrPush(player.playerState);
        }
        setSoundBasedOnStrength();
    }
    void setSoundBasedOnStrength()
    {
        switch (magnetStrength)
        {
            case < 1:
                playerInFieldAudioSource.volume = 0.5f;
                playerInFieldAudioSource.pitch = 1f;
                break;
            case < 5:
                playerInFieldAudioSource.volume = 0.6f;
                playerInFieldAudioSource.pitch = 1.67f;
                break;
            case < 20:
                playerInFieldAudioSource.volume = 0.7f;
                playerInFieldAudioSource.pitch = 2.33f;
                break;
            default:
                playerInFieldAudioSource.volume = 0.8f;
                playerInFieldAudioSource.pitch = 3f;
                break;
        }
    }
    public float CalculatePullOrPush(ChargeState targetCharge)
    {
        switch ((int)targetCharge * (int)magnetCharge)
        {
            case (-1):
                return hardPullMultiplier;
            case (0):
                return softPullMultiplier;
            case (1):
                return pushMultiplier;
            default:
                return 0f;
        }
    }

    // show the force by the velocity magnitude of the shader lines
    private void displayMagnitude(){
        m_SpriteRenderer.material.SetFloat("_Magnitude", magnetStrength/3f);
    }
    // set the direction of the shader lines
    private void setShaderDirection(){
        if(player.playerState == magnetCharge){
            m_SpriteRenderer.material.SetFloat("_Direction", -1);
        }
        else{
            m_SpriteRenderer.material.SetFloat("_Direction", 1);
        }
    }
    // update the direction of the shader lines
    private void changeDirection(){
        direction *= -1f;
        m_SpriteRenderer.material.SetFloat("_Direction", direction);
    }

    //sound
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            playerInFieldAudioSource.Stop();
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            Debug.Log(magnetStrength);
            playerInFieldAudioSource.Play();
        }
    }

}
