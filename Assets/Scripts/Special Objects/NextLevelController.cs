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
        fadeImage = GameObject.FindGameObjectWithTag("FadeImage")?.GetComponent<Image>();
        Debug.Log(fadeImage);
        iterationDuration = fadeDuration / fadeIterations;
    }
    private void Start()
    {
        if(fadeImage)
        {

            if (fadeImage.color.a > 0.99f)
            {
                StartCoroutine(enterCurrentLevel());
            }
        }
        else
        {
            Debug.Log("fadeimage not found");
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
            StartCoroutine(leaveCurrentLevel());
        }
    }
    IEnumerator leaveCurrentLevel()
    {
        Debug.Log("leave current level!");
        if (fadeImage)
        {
            for (int i = 0; i < fadeIterations; i++)
            {
                var temp = fadeImage.color;
                temp.a += iterationDuration;
                fadeImage.color = temp;
                yield return new WaitForSeconds(iterationDuration);
            }
        }
        else
        {
            Debug.Log("fadeimage not found");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    IEnumerator enterCurrentLevel()
    {
        Debug.Log("enter new level!");
        Color tempColor = fadeImage.color;
        while(fadeImage.color.a < 1) { 
            tempColor.a -= iterationDuration;
            fadeImage.color = tempColor;
            yield return new WaitForSeconds(iterationDuration);
        }
        tempColor.a = 1;
        fadeImage.color = tempColor;
    }
}
