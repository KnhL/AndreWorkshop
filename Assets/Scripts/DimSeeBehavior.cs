using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DimSeeBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject[] steamObjects; 

    [SerializeField]
    private FrequencySlider freqSlid;

    [SerializeField]
    private CameraBehavior camBehavior;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Animator clockAnimator;

    [SerializeField]
    private KeyCode radioSelect;

    private bool selected = false;

    private float delay;

    private bool localPlayerSeen;

    private void Update()
    {
        delay += Time.deltaTime;

        if(delay >= 1)
        {
            delay = 1;
        }


        if (Input.GetKeyDown(radioSelect) && delay >= 1)
        {
            selected = !selected;

            delay = 0;

            if (selected == true)
            {
                camBehavior.LockCur(false);

                animator.SetBool("RadioSelected", true);

                freqSlid.selected = true;
            }
            else if (selected == false)
            {
                camBehavior.LockCur(true);

                animator.SetBool("RadioSelected", false);

                freqSlid.selected = false;
            }
        }
    }

    public void ClockAnimStart(bool a)
    {
        clockAnimator.SetBool("PlayerSeen", a);
    }

    public void SteamStart(bool playerSeen)
    {
        //Debug.Log(playerSeen);
        localPlayerSeen = playerSeen;

        if (playerSeen == true)
        {
            StartCoroutine(SteamStartCou(playerSeen));
        }
        //else
        //{
        //    for (int cnt = 0; cnt < steamObjects.Length; cnt++)
        //    {
        //        steamObjects[cnt].GetComponent<VisualEffect>().SetFloat("SpawnRateMultiplier", 0);
        //    }
        //}
    }

    private IEnumerator SteamStartCou(bool playerSeen)
    {
        WaitForSeconds wfs = new WaitForSeconds(1f);

        if (playerSeen == true)
        {
            for (int cnt = 0; cnt < steamObjects.Length; cnt++)
            {
                steamObjects[cnt].GetComponent<VisualEffect>().SetFloat("SpawnRateMultiplier", 1);

                yield return wfs;
            }
        }
    }

    private void LateUpdate()
    {
        //Debug.Log(localPlayerSeen);

        if (localPlayerSeen == false)
        {
            for (int cnt = 0; cnt < steamObjects.Length; cnt++)
            {
                steamObjects[cnt].GetComponent<VisualEffect>().SetFloat("SpawnRateMultiplier", 0);
            }
        }
    }
}
