using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D bulletRb;

    public GameObject hitPrefab;
    public GameObject muzzlePrefab;

    void Start()
    {
        Instantiate(muzzlePrefab, gameObject.transform.position, hitPrefab.transform.rotation);
        bulletRb.velocity = transform.right * speed;
        StartCoroutine(destroyBullet());
    }
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(hitPrefab, gameObject.transform.position, hitPrefab.transform.rotation);
        Destroy(gameObject);
    }
}
