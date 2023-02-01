using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //portals
    public GameObject activeRespawnPortal;
    public GameObject[] pooledRedSplatters;
    public GameObject[] pooledBlueSplatters;
    public int amountToPool = 1000;
    public GameObject splatter;
    private int redPoolingCounter = 0;
    private int bluePoolingCounter = 0;

    //swap charge icon
    public Material swapChargeIconMaterial;
    public GameObject SwapChargeCooldownIcon;

    GameObject player;
    public PlayerController playerController;
    public PlayerMovement playerMovement;
    MagneticField[] magneticFields;


    private void Awake()
    {
        poolSplatters();
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerController.respawnPortalTransform = activeRespawnPortal.transform;
        activeRespawnPortal.SendMessage("activate", false);
        magneticFields = FindObjectsOfType<MagneticField>();
    }
    private void poolSplatters()
    {
        //GameObject splatters
        pooledRedSplatters = GameObject.FindGameObjectsWithTag("RedSplatter");
        pooledBlueSplatters = GameObject.FindGameObjectsWithTag("BlueSplatter");
        Vector3 splatterStartingPosition = new Vector3(0.0f, 0.0f, -1000.0f);

        if (pooledRedSplatters.Length == 0)
        {
            //splatter pooling
            pooledRedSplatters = new GameObject[amountToPool / 2];
            pooledBlueSplatters = new GameObject[amountToPool / 2];
            for (int i = 0; i < amountToPool / 2; i++)
            {
                pooledRedSplatters[i] = Instantiate(splatter, splatterStartingPosition, Quaternion.identity);
                pooledRedSplatters[i].SendMessage("pickColor", ChargeState.Red);
                pooledRedSplatters[i].tag = "RedSplatter";
                DontDestroyOnLoad(pooledRedSplatters[i]);
            }
            for (int i = 0; i < amountToPool / 2; i++)
            {
                pooledBlueSplatters[i] = Instantiate(splatter, splatterStartingPosition, Quaternion.identity);
                pooledBlueSplatters[i].SendMessage("pickColor", ChargeState.Blue);
                pooledBlueSplatters[i].tag = "BlueSplatter";
                DontDestroyOnLoad(pooledBlueSplatters[i]);
            }
        }
        else
        {
            for (int i = 0; i < amountToPool / 2; i++)
            {
                pooledRedSplatters[i].transform.position = splatterStartingPosition;
                DontDestroyOnLoad(pooledRedSplatters[i]);
            }
            for (int i = 0; i < amountToPool / 2; i++)
            {
                pooledBlueSplatters[i].transform.position = splatterStartingPosition;
                DontDestroyOnLoad(pooledBlueSplatters[i]);

            }
        }
    }
    // Change the active Portal
    void setActiveRespawnPortal(GameObject portal)
    {
        if (portal != activeRespawnPortal){
            activeRespawnPortal.SendMessage("deactivate");
            activeRespawnPortal = portal;
            activeRespawnPortal.SendMessage("activate", true);
        }
        playerController.respawnPortalTransform = activeRespawnPortal.transform;
    }
    void updateDirectionOfFields(){
        foreach(MagneticField field in magneticFields)
        {
            field.SendMessage("changeDirection");
        }
    }
    public GameObject getNextSplatter(ChargeState color)
    {
        if (color == ChargeState.Red) { 
            redPoolingCounter += 1;
            return pooledRedSplatters[redPoolingCounter % pooledRedSplatters.Length];
        }
        else
        {
            bluePoolingCounter += 1;
            return pooledBlueSplatters[bluePoolingCounter % pooledBlueSplatters.Length];
        }
    }
    public void killPlayer(float respawnTimer)
    {
        StartCoroutine(killPlayerCoroutine(respawnTimer));
    }
    public void rotateAndRecolorChargeIcon()
    {
        StartCoroutine(rotateAndRecolorChargeIconCoroutine());
    }
    private IEnumerator rotateAndRecolorChargeIconCoroutine()
    {
        float rotation = 1.8f;
        int iterations = Mathf.CeilToInt(180f / rotation);
        float timeSpanLength = 0.5f / iterations;
        float colorChangeIncrement = 1f / iterations;
        float colorChange = swapChargeIconMaterial.GetFloat("_IconCharge");
        if (colorChange == 1)
        {
            for (int i = 0; i < iterations; i++)
            {
                SwapChargeCooldownIcon.transform.Rotate(new Vector3(0, 0f, -rotation));
                colorChange -= colorChangeIncrement;
                swapChargeIconMaterial.SetFloat("_IconCharge", colorChange);
                yield return new WaitForSeconds(timeSpanLength);
            }
        }
        if (colorChange == 0)
        {
            for (int i = 0; i < iterations; i++)
            {
                SwapChargeCooldownIcon.transform.Rotate(new Vector3(0, 0f, +rotation));
                colorChange += colorChangeIncrement;
                swapChargeIconMaterial.SetFloat("_IconCharge", colorChange);
                yield return new WaitForSeconds(timeSpanLength);
            }
        }
        swapChargeIconMaterial.SetFloat("_IconCharge", Mathf.Round(colorChange));
    }

    private IEnumerator killPlayerCoroutine(float respawnTimer)
    {
        playerMovement.playerTrail.emitting = false;

        playerController.powerUpFireRed.Stop();
        playerController.powerUpFireBlue.Stop();
        player.SetActive(false);
        yield return new WaitForSeconds(respawnTimer);
        player.SetActive(true);
        playerController.respawn();
    }
}
