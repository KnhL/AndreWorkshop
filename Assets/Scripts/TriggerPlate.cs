using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    //Triggers on collide
    [SerializeField]
    GameObject Activate;

    void OnTriggerEnter(Collider col) 
    {
        Activate.SetActive(true);
    }
}
