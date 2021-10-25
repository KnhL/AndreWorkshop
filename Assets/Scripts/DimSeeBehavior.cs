using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimSeeBehavior : MonoBehaviour
{
    [SerializeField]
    private FrequencySlider freqSlid;

    [SerializeField]
    private CameraBehavior camBehavior;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private KeyCode radioSelect;

    private bool selected = false;

    private float delay;

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


}
