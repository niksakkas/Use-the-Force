using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class PortalController : MonoBehaviour
{
    public bool isActive;
    GameController gameController;
    SpriteRenderer m_SpriteRenderer;
    public Color inactiveLightColor = new Color(0.61f, 0.16f, 1f, 1f);
    public Color activeLightColor = new Color(1, 0.16f, 0.98f, 1f);
    public UnityEngine.Rendering.Universal.Light2D portalLight;
    float change = 1;
    float basicLightInnerRadius = 0;
    float basicLightOuterRadius = 0;

    // Sound
    [SerializeField] private AudioSource activationAudioSource;
    [SerializeField] private AudioSource constantAudioSource;

    private void Awake()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        portalLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        portalLight.color = Color.Lerp(activeLightColor, inactiveLightColor, change);
        basicLightInnerRadius = portalLight.pointLightInnerRadius;
        basicLightOuterRadius = portalLight.pointLightOuterRadius;
    }
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }
    private void Update()
    {
        // pingpong portal's light radius
        portalLight.pointLightInnerRadius = basicLightInnerRadius * (1 + Mathf.PingPong(Time.fixedTime * 0.2f, 0.2f));
        portalLight.pointLightOuterRadius = basicLightOuterRadius * (1 + Mathf.PingPong(Time.fixedTime * 0.2f, 0.2f));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.GetComponent<Collider2D>().tag == "Player")
        {
            gameController.SendMessage("setActiveRespawnPortal", gameObject);
        }
    }

    private void activate(bool playSound = true)
    {

        StartCoroutine(activateCoroutine(playSound));
    }
    public void deactivate(){
        change = 1;
        portalLight.color = Color.Lerp(activeLightColor, inactiveLightColor, change);
        m_SpriteRenderer.material.SetFloat("_TwirlSpeed", 0.2f);
        m_SpriteRenderer.material.SetFloat("_ColorTransition", 1);
    }

    public IEnumerator activateCoroutine(bool playSound)
    {
        if (playSound)
        {
            activationAudioSource.Play();
        }
        change = 1;
        m_SpriteRenderer.material.SetFloat("_TwirlSpeed", 0.6f);
        while (change > 0f)
        {
            portalLight.color = Color.Lerp(activeLightColor, inactiveLightColor , change);
            m_SpriteRenderer.material.SetFloat("_ColorTransition", change);
            change -= 2 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

}
