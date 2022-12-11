using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuManager : MonoBehaviour
{
    public int startScene;

    // Moving Magnet stuff
    public GameObject menuMagnet;

    //rotation
    float r = 23f;
    float alpha = 0f;
    float X;
    float Y;
    //Color
    SpriteRenderer m_spriteRenderer;
    Color lerpedFieldColor;

    private void Awake()
    {
        m_spriteRenderer = menuMagnet.GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
    }
    private void Update()
    {
        alpha += 10;
        X = (r * Mathf.Cos(alpha * 0.0001f));
        Y = (r * Mathf.Sin(alpha * 0.0001f));
        menuMagnet.transform.position = new Vector3(X, Y, 0);
        lerpedFieldColor = Color.Lerp(GlobalVariables.blueColor, Color.red, Mathf.PingPong(Time.time * 0.1f, 1));
        m_spriteRenderer.material.SetColor("_FieldColor", lerpedFieldColor);
        m_spriteRenderer.material.SetColor("_LinesColor", lerpedFieldColor);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(startScene);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
