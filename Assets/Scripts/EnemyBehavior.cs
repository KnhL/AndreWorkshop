using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public bool seenPlayer = false;

    public PlayerController player;

    [SerializeField]
    private NavMeshAgent navAgent;

    [SerializeField]
    private GameObject eye;

    [SerializeField]
    private Transform eyeGoToTrans;

    [SerializeField]
    private Material eyeMat;

    [SerializeField]
    private Material irisMat;

    private Quaternion eyeStartQuat;

    private Vector3 playerLastPos;

    private Vector3 targetDestination;

    private float timer;

    private float glowTimer;

    bool waitForSpawn = false;
    

    private void Start()
    {
        player = GameObject.Find("Player(Clone)").GetComponent<PlayerController>();

        targetDestination = this.transform.position;

        eyeStartQuat = eye.transform.localRotation;

        Debug.Log(eyeStartQuat);
    }

    private void Update()
    {
        if(seenPlayer == true)
        {
            eyeGoToTrans.transform.LookAt(player.transform);
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eyeGoToTrans.rotation, 0.05f);

            Vector4 newTillingValue = Vector4.Lerp(eyeMat.GetVector("Tilling"), new Vector4(0, 0, 0, 0), 0.05f);
            eyeMat.SetVector("Tilling", newTillingValue);

            float newHeightValue = Mathf.Lerp(eyeMat.GetFloat("Height"), 2, 0.05f);
            eyeMat.SetFloat("Height", newHeightValue);

            glowTimer += Time.deltaTime;

            if (glowTimer >= 0.2f)
            {
                glowTimer = 0.2f;

                float newGlowMultiplier = Mathf.Lerp(irisMat.GetFloat("GlowMultiplier"), 200, 0.1f);
                irisMat.SetFloat("GlowMultiplier", newGlowMultiplier);
            }

            playerLastPos = player.transform.position;

            targetDestination = playerLastPos;
            Debug.Log(targetDestination);

            timer = 0;
            
        }
        else
        {
            Vector4 newTillingValue = Vector4.Lerp(eyeMat.GetVector("Tilling"), new Vector4(1, 1, 0, 0), 0.05f);
            eyeMat.SetVector("Tilling", newTillingValue);

            float newHeightValue = Mathf.Lerp(eyeMat.GetFloat("Height"), 1, 0.05f);
            eyeMat.SetFloat("Height", newHeightValue);

            float newGlowMultiplier = Mathf.Lerp(irisMat.GetFloat("GlowMultiplier"), 20, 0.1f);
            irisMat.SetFloat("GlowMultiplier", newGlowMultiplier);

            timer += Time.deltaTime;

            glowTimer = 0;

            if(timer >= 5)
            {
                timer = 5;

                eye.transform.localRotation = Quaternion.Lerp(eye.transform.localRotation, eyeStartQuat, 0.05f);
            }
        }

        if (!waitForSpawn && new Vector3(transform.position.x, 0, transform.position.z) == new Vector3(targetDestination.x, 0, targetDestination.z))
        {
            waitForSpawn = true;
            StartCoroutine(spawnNewPos());
        }
        else if (!waitForSpawn)
        {
            navAgent.SetDestination(targetDestination);
        }
    }

    IEnumerator spawnNewPos()
    {
        while (waitForSpawn)
        {
            if (player.route.Count >= 30)
            {
                Debug.Log("Enemy Start");
                int wait = Random.Range(3, 6);
                Debug.Log("Wait time: " + wait + " and go time = " + Time.deltaTime + wait);
                yield return new WaitForSeconds(wait);
                int nr = Random.Range(10, player.route.Count - 10);
                Debug.Log("Route nr: " + player.route[nr]);
                Debug.Log("Pre Pos: " + transform.position);
                transform.position = player.route[nr].transform.position + Vector3.up;
                Debug.Log("Post Pos: " + transform.position);
                playerLastPos = player.transform.position;
                player.route.Clear();
                waitForSpawn = false;
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
        }

    }
}
