using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D bulletRb;
    // Start is called before the first frame update
    void Start()
    {
        bulletRb.velocity = transform.right * speed;
        StartCoroutine(destroyBullet());
    }
    IEnumerator destroyBullet()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

}
