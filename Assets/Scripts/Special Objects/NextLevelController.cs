using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class NextLevelController : MonoBehaviour
{
    public Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private int fadeIterations = 100;
    [SerializeField] private float iterationDuration;

    [SerializeField] private AudioSource nextLevelAudioSource;

    private void Awake()
    {
        iterationDuration = fadeDuration / fadeIterations;
    }
    private void Update()
    {
        // this is to advance through levels easily during debugging. Comment this block when done with debugging
        if (Input.GetButtonDown("MainAbility"))
        {
            StartCoroutine(leaveCurrentLevel());
        }
    }
    private void Start()
    {
        nextLevelAudioSource = GameObject.FindGameObjectWithTag("LevelCompletedSound")?.GetComponent<AudioSource>();
        fadeImage = GameObject.FindGameObjectWithTag("FadeImage")?.GetComponent<Image>();

        if (fadeImage)
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
        if (nextLevelAudioSource)
        {
            nextLevelAudioSource.Play();
        }
        if (fadeImage)
        {
            Color tempColor = fadeImage.color;
            while (fadeImage.color.a < 1)
            {
                tempColor.a += iterationDuration;
                fadeImage.color = tempColor;
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
        while(fadeImage.color.a > 0) 
        {
            tempColor.a -= iterationDuration;
            fadeImage.color = tempColor;
            yield return new WaitForSeconds(iterationDuration);
        }
        tempColor.a = 0;
        fadeImage.color = tempColor;
    }

}
