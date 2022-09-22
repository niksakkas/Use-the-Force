using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{

    public float pushMultiplier;       // when magnet and target are both blue or red
    public float hardPullMultiplier;   // when magnet and target are opposite colors
    public float softPullMultiplier;   // when either the magnet or the target are purple
    [Range(0, 30)][SerializeField] public float magnetStrength;   // (0,inf) Power of the magnet
    public ChargeState magnetCharge;
    SpriteRenderer m_SpriteRenderer;
    PlayerController player;
    float direction;
    

    private void Awake(){
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        direction = CalculatePullOrPush(player.playerState);
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        displayMagnitude();
        setShaderDirection();
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

}
