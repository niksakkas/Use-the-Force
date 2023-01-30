using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndMenuManager : MonoBehaviour
{
    private AudioSource buttonClickSound;


    private void Awake()
    {
        buttonClickSound = GameObject.FindGameObjectWithTag("ButtonSound").GetComponent<AudioSource>();
        Destroy(GameObject.FindGameObjectWithTag("StaticElements"));
        Destroy(GameObject.FindGameObjectWithTag("MusicPlayer"));
    }
    public void BackToStartingScreen()
    {
        buttonClickSound.Play();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        buttonClickSound.Play();
        Application.Quit();
    }
}
