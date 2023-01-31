using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Slider slider;
    private AudioSource buttonClickSound;
    private MusicController musicController;

    private void Awake()
    {
        musicController = GameObject.FindGameObjectWithTag("MusicPlayer")?.GetComponent<MusicController>();
        if (musicController)
        {
            slider.value = musicController.GetComponent<AudioSource>().volume;
        }
        buttonClickSound = GameObject.FindGameObjectWithTag("ButtonSound")?.GetComponent<AudioSource>();
    }
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
        if (buttonClickSound)
        {
            buttonClickSound.Play();
        }
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public bool IsPaused()
    {
        return Time.timeScale == 0f;
    }
    public void Home()
    {
        if (buttonClickSound)
        {
            buttonClickSound.Play();
        }
        Time.timeScale = 1f;
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
    public void changeMusicVolume()
    {
        if (musicController)
        {
            musicController.GetComponent<AudioSource>().volume = slider.value;
        }
    }
    
}