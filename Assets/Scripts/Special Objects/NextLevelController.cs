using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelController : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    private int fadeIterations = 100;
    private float iterationDuration;
    private void Awake()
    {
        fadeImage = GameObject.FindGameObjectWithTag("FadeImage").GetComponent<Image>();
        iterationDuration = fadeDuration / fadeIterations;
    }
    private void Start()
    {
        if (fadeImage.color.a > 0.99f)
        {
            StartCoroutine(enterCurrentLevel());
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            StartCoroutine(leaveCurrentLevel());
        }
    }
    IEnumerator leaveCurrentLevel()
    {
        Debug.Log("leave current level!");
        for (int i = 0; i < fadeIterations; i++)
        {
            var temp = fadeImage.color;
            temp.a += iterationDuration;
            fadeImage.color = temp;
            yield return new WaitForSeconds(iterationDuration);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator enterCurrentLevel()
    {
        Debug.Log("enter new level!");
        for (int i = 0; i < fadeIterations; i++)
        {
            var tempColor = fadeImage.color;
            tempColor.a -= iterationDuration;
            fadeImage.color = tempColor;
            yield return new WaitForSeconds(iterationDuration);
        }
    }
}
