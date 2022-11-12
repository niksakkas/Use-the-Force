using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D bulletRb;

    public GameObject hitPrefab;
    public GameObject muzzlePrefab;

    private bool hasHit = false;
    private GameObject newMuzzle;
    private GameObject newHit;

    private UnityEngine.Rendering.Universal.Light2D bulletLight;

    void Start()
    {
        bulletLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        newMuzzle = Instantiate(muzzlePrefab, gameObject.transform.position, hitPrefab.transform.rotation);
        bulletRb.velocity = transform.right * speed;
        StartCoroutine(destroyAllSoon(10));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hasHit == false)
        {
            hasHit = true;
            newHit = Instantiate(hitPrefab, gameObject.transform.position, hitPrefab.transform.rotation);
            StartCoroutine(destroyAllSoon(3));
            hideBullet();
        }
    }
    private void destroyAll()
    {
        if (newHit)
        {
            Destroy(newHit);
        }
        Destroy(newMuzzle);
        Destroy(gameObject);
    }
    IEnumerator destroyAllSoon(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(true);
        destroyAll();
    }

    private void hideBullet()
    {
        bulletLight.enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }
}
