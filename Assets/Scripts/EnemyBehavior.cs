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

    private Transform eyeStartTrans;

    private Vector3 playerLastPos;
    

    private void Start()
    {
        eyeStartTrans = eye.transform;
    }

    private void Update()
    {
        if(seenPlayer == true)
        {
            eyeGoToTrans.transform.LookAt(player.transform);
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eyeGoToTrans.rotation, 0.05f);

            playerLastPos = player.transform.position;
            
        }
        else
        {
            eye.transform.rotation = Quaternion.Lerp(eye.transform.rotation, eyeStartTrans.rotation, 0.05f) ;
        }
    }


}
