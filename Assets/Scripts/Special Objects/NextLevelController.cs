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
        iterationDuration = fadeDuration / fadeIterations;
    }
    private void Update()
    {
        // this is to advance through levels easily during debugging. TODO: remove when done
        if (Input.GetButtonDown("MainAbility"))
        {
            StartCoroutine(leaveCurrentLevel());
        }
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
        Color tempColor = fadeImage.color;
        while(fadeImage.color.a > 0) {
            tempColor.a -= iterationDuration;
            fadeImage.color = tempColor;
            yield return new WaitForSeconds(iterationDuration);
        }
        tempColor.a = 0;
        fadeImage.color = tempColor;
    }

}
