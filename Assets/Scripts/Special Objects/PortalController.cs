using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public bool isActive;
    GameController gameController;
    SpriteRenderer m_SpriteRenderer;
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

    private void activate(){
        StartCoroutine(activateCoroutine());
    }
    public void deactivate(){

        m_SpriteRenderer.material.SetFloat("_TwirlSpeed", 0.2f);
        m_SpriteRenderer.material.SetFloat("_ColorTransition", 1);
    }

    public IEnumerator activateCoroutine()
    {
        change = 1;
        m_SpriteRenderer.material.SetFloat("_TwirlSpeed", 0.6f);
        while (change > 0f)
        {
            m_SpriteRenderer.material.SetFloat("_ColorTransition", change);
            change -= 2 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
