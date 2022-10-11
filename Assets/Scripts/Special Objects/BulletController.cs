using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D bulletRb;

    public GameObject hitPrefab;
    public GameObject muzzlePrefab;

    GameObject newMuzzle;
    GameObject newHit;

    void Start()
    {
        newMuzzle = Instantiate(muzzlePrefab, gameObject.transform.position, hitPrefab.transform.rotation);
        bulletRb.velocity = transform.right * speed;
        StartCoroutine(destroyObjectsSoon(10));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        newHit = Instantiate(hitPrefab, gameObject.transform.position, hitPrefab.transform.rotation);
    }

    IEnumerator destroyObjectsSoon(float time)
    {
        yield return new WaitForSeconds(time);
        if (newHit)
        {
            Destroy(newHit);
        }
        Destroy(newMuzzle);
        Destroy(gameObject);
    }
}
