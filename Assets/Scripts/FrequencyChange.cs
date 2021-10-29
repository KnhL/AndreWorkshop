using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencyChange : MonoBehaviour
{
    [SerializeField]
    private FrequencySlider radioFreq;

    [SerializeField]
    [Range(0, 100)]
    private float zoneFreq;

    private void Start()
    {
        zoneFreq = Random.Range(0, 100);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("RadioSlider"))
        {
            Debug.Log("FreqChange");


            radioFreq = other.gameObject.GetComponent<FrequencySlider>();

            radioFreq.wantedFreq = zoneFreq;
        }
    }


}
