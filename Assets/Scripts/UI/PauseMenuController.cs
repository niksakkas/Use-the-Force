using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuCanvas;
    [SerializeField] GameObject pauseMenu;
    private AudioSource buttonClickSound;
    // Music Slider
    [SerializeField] Slider musicSlider;
    private MusicController musicController;
    // Sound Slider
    [SerializeField] Slider soundSlider;
    AudioSource[] soundObjects;
    float[] soundObjectsStartingVolume;

    private void Awake()
    {
        DontDestroyOnLoad(pauseMenu);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        getAllSceneSoundEffects();
        changeSoundEffectsVolume();
    }

    private void Start()
    {
        musicController = GameObject.FindGameObjectWithTag("MusicPlayer")?.GetComponent<MusicController>();
        getAllSceneSoundEffects();
        changeSoundEffectsVolume();
        setMusicVolume();
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
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
    }
    private void getAllSceneSoundEffects(){
        soundObjects = FindObjectsOfType<AudioSource>();
        List<AudioSource> tempSoundObjectList = new List<AudioSource>(soundObjects);
        List<float> tempVolumeList = new List<float>();

        foreach (AudioSource soundObject in soundObjects.ToList())
        {
            // Music player is controlled by a different slider
            if (soundObject.gameObject.tag != "MusicPlayer")
            {
                tempVolumeList.Add(soundObject.volume);
            }
            else
            {
                tempSoundObjectList.Remove(soundObject);
            }
        };
        soundObjects = tempSoundObjectList.ToArray();
        soundObjectsStartingVolume = tempVolumeList.ToArray();
    }
    public void Resume()
    {
        if (buttonClickSound)
        {
            buttonClickSound.Play();
        }
        pauseMenuCanvas.SetActive(false);
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
    public void setMusicVolume()
    {
        if (musicController)
        {
            musicController.GetComponent<AudioSource>().volume = musicSlider.value;
        }
    }
    public void changeSoundEffectsVolume()
    {
        for (int i = 0; i < soundObjects.Length; i++)
        {
            soundObjects[i].volume = soundObjectsStartingVolume[i] * soundSlider.value;
        }
        
    }
}