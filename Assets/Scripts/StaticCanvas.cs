using UnityEngine.SceneManagement;

using UnityEngine;
using TMPro;

public class StaticCanvas : MonoBehaviour
{
    [SerializeField] private int deaths = 0;
    [SerializeField] private TMP_Text deathCounterText;
    [SerializeField] private TMP_Text tempDeathCounterText;
    private bool toogle = true;
    [SerializeField] AudioSource nextLevelSound;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        tempDeathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.alignment = TextAlignmentOptions.Left;
        deathCounterText.text = "Deaths: 0";
        tempDeathCounterText.text = "Deaths: 0";
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
