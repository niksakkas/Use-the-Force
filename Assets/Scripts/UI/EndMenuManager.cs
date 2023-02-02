using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndMenuManager : MonoBehaviour
{
    private AudioSource buttonClickSound;
    GameObject pauseMenu;
    GameObject musicPlayer;

    private void Awake()
    {
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        if (pauseMenu)
        {
            Destroy(pauseMenu);
        }
    }
    private void Start()
    {
        buttonClickSound = GameObject.FindGameObjectWithTag("ButtonSound")?.GetComponent<AudioSource>();
        Destroy(GameObject.FindGameObjectWithTag("StaticElements"));
        StartCoroutine(stopMusic());
    }

    private IEnumerator stopMusic()
    {
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer");
        if (musicPlayer)
        {
            AudioSource music = musicPlayer.GetComponent<AudioSource>();
            float startingSound = music.volume;
            for (int i = 0; i < 20; i++)
            {
                music.volume = (20f - (float)i) * startingSound / 20f;
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(musicPlayer);
        }
    }
    public void BackToStartingScreen()
    {
        if (buttonClickSound)
        {
            buttonClickSound.Play();
        }
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        if (buttonClickSound)
        {
            buttonClickSound.Play();
        }
        Application.Quit();
    }
}
