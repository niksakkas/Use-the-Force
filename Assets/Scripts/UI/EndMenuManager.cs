using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndMenuManager : MonoBehaviour
{
    private void Awake()
    {
        Destroy(GameObject.FindGameObjectWithTag("StaticElements"));
        Destroy(GameObject.FindGameObjectWithTag("MusicPlayer"));
    }
    public void BackToStartingScreen()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
