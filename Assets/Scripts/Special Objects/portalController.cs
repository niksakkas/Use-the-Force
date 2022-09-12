using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalController : MonoBehaviour
{
    gameController gameController;
    SpriteRenderer m_SpriteRenderer;
    
    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Collider2D>().tag == "Player")
        {
            gameController.SendMessage("setActiveRespawnPortal", gameObject);
        }
    }
    private void activate(){
        m_SpriteRenderer.material.SetFloat("_Transition", 0);
    }
    private void deactivate(){
        m_SpriteRenderer.material.SetFloat("_Transition", 1);
    }
}
