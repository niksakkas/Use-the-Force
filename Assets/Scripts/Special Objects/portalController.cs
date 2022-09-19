using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public bool isActive;
    GameController gameController;
    SpriteRenderer m_SpriteRenderer;
    bool isActivating = false;
    float change = 1;
    
    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Collider2D>().tag == "Player")
        {
            gameController.SendMessage("setActiveRespawnPortal", gameObject);
        }
    }
    private void Update(){
        if(isActivating){
            if(change > 0f){
                m_SpriteRenderer.material.SetFloat("_Transition", change);
                change -= 2*Time.deltaTime;
            }
            else{
                change = 0f;
                m_SpriteRenderer.material.SetFloat("_Transition", change);
                isActivating = false;
            }
        }
    }
    private void activate(){
        if(isActivating == false){
            change = 1;
            isActivating = true;
        }

    }
    private void deactivate(){
        m_SpriteRenderer.material.SetFloat("_Transition", 1);
    }
}
