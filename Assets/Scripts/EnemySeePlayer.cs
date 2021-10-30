using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySeePlayer : MonoBehaviour
{
    [SerializeField]
    private EnemyBehavior enemyScript;

    [SerializeField]
    private float timerMax;

    [SerializeField]
    private LayerMask raycastLayer;

    [SerializeField]
    private float timer;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            RaycastHit hit;

            Debug.DrawRay(this.transform.position, other.transform.position - this.transform.position, Color.white, Vector3.Distance(this.transform.position, other.transform.position));

            if (Physics.Raycast(this.transform.position, other.transform.position - this.transform.position, out hit, Vector3.Distance(this.transform.position, other.transform.position), raycastLayer))
            {
                Debug.Log(hit.collider.name);

                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    if (timer <= timerMax)
                    {
                        timer += Time.deltaTime;
                    }

                    if (timer >= timerMax)
                    {
                        timer = timerMax;

                        enemyScript.player = other.gameObject;

                        enemyScript.seenPlayer = true;
                    }
                }
                else
                {
                    timer -= Time.deltaTime;

                    if (timer < -0.001)
                    {
                        timer = 0;
                    }

                    enemyScript.seenPlayer = false;
                }

            }
        }
    }
}
