using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>())
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>())
        {
            other.gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
