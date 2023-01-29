using UnityEngine;

public class MusicController : MonoBehaviour
{    void Awake()
    {
        // if there is no other music player, activate this one, otherwise destroy it
        if(GameObject.FindGameObjectsWithTag("MusicPlayer").Length == 1)
        {
            GetComponent<AudioSource>().enabled = true;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
