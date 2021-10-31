using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool seenPlayer = false;

    public GameObject player;

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

    private float timer;
    

    private void Start()
    {
        eyeStartQuat = eye.transform.rotation;
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

            float newGlowMultiplier = Mathf.Lerp(irisMat.GetFloat("GlowMultiplier"), 200, 0.1f);
            irisMat.SetFloat("GlowMultiplier", newGlowMultiplier);

            playerLastPos = player.transform.position;

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

            if(timer >= 5)
            {
                timer = 5;

                eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eyeStartQuat, 0.05f);
            }
        }
    }
}
