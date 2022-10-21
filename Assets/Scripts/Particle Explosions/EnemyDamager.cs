using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(removeCollider());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
            collision.gameObject.SetActive(false);
    }

    IEnumerator removeCollider()
    {
        yield return new WaitForSeconds(0.4f);
        gameObject.GetComponent<Collider2D>().enabled = false;
    }
}
