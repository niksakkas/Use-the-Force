using UnityEngine.SceneManagement;

using UnityEngine;
using TMPro;

public class StaticElements : MonoBehaviour
{
    [SerializeField] private int deaths = 0;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text deathCounterText;
    [SerializeField] private TMP_Text tempDeathCounterText;
    private bool toogle = true;
    [SerializeField] AudioSource nextLevelSound;
    Scene scene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        updateLevelText();
    }
    private void Start()
    {
        tempDeathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.text = "Deaths: 0";
        tempDeathCounterText.text = "Deaths: 0";
        updateLevelText();
    }
    private void updateLevelText()
    {
        scene = SceneManager.GetActiveScene();
        levelText.text = "Level " + scene.buildIndex;
    }
    public void addDeath()
    {
        deaths++;

        if (toogle == true)
        {
            tempDeathCounterText.text = "Deaths: " + deaths;
            deathCounterText.CrossFadeAlpha(0f, 1f, false);
            tempDeathCounterText.CrossFadeAlpha(1f, 1f, false);
            toogle = false;
        }
        else
        {
            deathCounterText.text = "Deaths: " + deaths;
            deathCounterText.CrossFadeAlpha(1f, 1f, false);
            tempDeathCounterText.CrossFadeAlpha(0f, 1f, false);
            toogle = true;
        }
    }
}
