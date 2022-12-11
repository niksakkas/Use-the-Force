using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{


    [SerializeField] GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (IsPaused())
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public bool IsPaused()
    {
        return Time.timeScale == 0f;
    }
    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}