using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityFieldController : MonoBehaviour
{
    public Rigidbody2D playerRB;
    float gravityScale;
    // Sound
    private float baseVolume;
    [SerializeField] private AudioSource playerInFieldAudioSource;

    private void Awake()
    {
        gravityScale = playerRB.gravityScale;
        baseVolume = playerInFieldAudioSource.volume;
    }
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            disableVerticalControl(collider.gameObject);
            applyPlayerGravityPull(collider.attachedRigidbody);
            StartCoroutine(stopSound());
        }
    }
    IEnumerator stopSound()
    {
        int numOfIncrements = 5;
        float volumeIncrement = playerInFieldAudioSource.volume * (1f / numOfIncrements);
        for (int i = 0; i < numOfIncrements; i++)
        {
            playerInFieldAudioSource.volume -= volumeIncrement;
            yield return new WaitForSeconds(0.1f);
        }
        playerInFieldAudioSource.Stop();

    }
    private void startSound()
    {
        playerInFieldAudioSource.Play();
        playerInFieldAudioSource.volume = baseVolume;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            enableVerticalControl(collider.gameObject);
            removePlayerGravityPull(collider.attachedRigidbody);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.GetComponent<Collider2D>().tag == "Player")
        {
            startSound();
        }
    }
    private void removePlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.GetComponent<Rigidbody2D>().gravityScale = 0; // remove gravity pull
    }
    private void applyPlayerGravityPull(Rigidbody2D playerRB)
    {
        playerRB.velocity = Vector2.zero;
        playerRB.GetComponent<Rigidbody2D>().gravityScale = gravityScale; // apply gravity pull
    }

    private void enableVerticalControl(GameObject player)
    {
        player.GetComponent<CharacterController2D>().verticalControl = true;
    }

    private void disableVerticalControl(GameObject player)
    {
        player.GetComponent<CharacterController2D>().verticalControl = false;
    }
}
