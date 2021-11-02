using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayAudioSource : MonoBehaviour
{
    [SerializeField]
    private bool isOnTarget;

    [SerializeField]
    private VisualEffect vfx;

    [SerializeField]
    private string vfxTargetString;

    [SerializeField]
    private AudioSource audioS;

    [SerializeField]
    private float audioMax;

    private float vfxStart;


    private void Start()
    {
        if (vfx != null)
        {
            vfxStart = vfx.GetFloat(vfxTargetString);
        }

        if (isOnTarget == true)
        {
            if (this.GetComponent<VisualEffect>().isActiveAndEnabled == true)
            {
                vfx = this.GetComponent<VisualEffect>();
            }

            if (this.GetComponent<AudioSource>().isActiveAndEnabled == true)
            {
                audioS = this.GetComponent<AudioSource>();
            }

        }

    }

    void Update()
    {
        if(vfx != null)
        {
            float vfxProcent = (vfx.GetFloat(vfxTargetString) / vfxStart) * 100;

            float newAudioVolume = (audioMax / 100) * vfxProcent;

            audioS.volume = Mathf.Lerp(audioS.volume, newAudioVolume, 0.2f);

        }
    }
}
