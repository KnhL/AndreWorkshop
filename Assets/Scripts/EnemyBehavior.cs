using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehavior : MonoBehaviour
{
    public bool seenPlayer = false;

    public GameObject player;

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
    

    private void Start()
    {
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

            if (glowTimer >= 0.5f)
            {
                glowTimer = 0.5f;

                float newGlowMultiplier = Mathf.Lerp(irisMat.GetFloat("GlowMultiplier"), 200, 0.2f);
                irisMat.SetFloat("GlowMultiplier", newGlowMultiplier);
            }

            playerLastPos = player.transform.position;

            targetDestination = playerLastPos;

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

        navAgent.SetDestination(targetDestination);
    }
}
