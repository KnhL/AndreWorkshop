using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeenScript : MonoBehaviour
{
    [SerializeField]
    private AudioSource isVisibleAudio;

    [SerializeField]
    private float isVisibleTimer = 10;

    [SerializeField]
    private LayerMask raycastLayer;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        isVisibleAudio = GameObject.Find("EnemyIsVisibleAudio").GetComponent<AudioSource>();
    }

    private void Update()
    {
        isVisibleTimer += Time.deltaTime;

        if (isVisibleTimer >= 10)
        {
            isVisibleTimer = 10;
        }
    }

    private void OnWillRenderObject()
    {
        RaycastHit hit;

        Debug.Log("test");

        Debug.DrawRay(this.transform.position, player.gameObject.transform.position - this.transform.position, Color.red, Vector3.Distance(this.transform.position, player.gameObject.transform.position));

        if (Physics.Raycast(this.transform.position, player.gameObject.transform.position - this.transform.position, out hit, Vector3.Distance(this.transform.position, player.gameObject.transform.position), raycastLayer))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                Debug.Log("test hit " + hit.collider.name);

                if (isVisibleTimer >= 10)
                {
                    isVisibleAudio.Play();

                    isVisibleTimer = 0;
                }
            }
        }
    }
}
