using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public int startScene;

    // Moving Magnet stuff
    [SerializeField] private GameObject menuMagnet;
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D magnetLight;

    // Rotation
    float r = 23f;
    float alpha = 0f;
    float X;
    float Y;
    // Color
    SpriteRenderer m_spriteRenderer;
    Color lerpedFieldColor;
    // Button Sound
    GameObject buttonSound;
    GameObject pauseMenu;
    // Light
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D SceneLight;

    private void Start()
    {
        // destroy PauseMenu if it exists
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        if (pauseMenu)
        {
            Destroy(pauseMenu);
        }
        // destroy ButtonSound if it exists
        GameObject[] buttonSounds = GameObject.FindGameObjectsWithTag("ButtonSound");
        if (buttonSounds.Length == 2)
        {
            Destroy(buttonSounds[1]);
        }
        buttonSound = buttonSounds[0];
        DontDestroyOnLoad(buttonSound);

        // destroy StaticElements gameObject if it exists
        GameObject StaticElements = GameObject.FindGameObjectWithTag("StaticElements");
        if (StaticElements)
        {
            Destroy(StaticElements);
        }
        magnetLight = menuMagnet.GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();
        m_spriteRenderer = menuMagnet.GetComponentInChildren<SpriteRenderer>();
        
        // enable scene light 
        SceneLight.enabled = true;
    }

    private void Update()
    {
        alpha += 10;
        X = (r * Mathf.Cos(alpha * 0.00003f));
        Y = (r * Mathf.Sin(alpha * 0.00003f));
        menuMagnet.transform.position = new Vector3(X, Y, 0);
        magnetLight.color = lerpedFieldColor;
        lerpedFieldColor = Color.Lerp(GlobalVariables.blueColor, Color.red, Mathf.PingPong(Time.time * 0.1f, 1));
        m_spriteRenderer.material.SetColor("_FieldColor", lerpedFieldColor);
        m_spriteRenderer.material.SetColor("_LinesColor", lerpedFieldColor);
    }

    public void StartGame()
    {
        buttonSound.GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(startScene);
    }
    public void QuitGame()
    {
        buttonSound.GetComponent<AudioSource>().Play();
        Application.Quit();
    }
}
