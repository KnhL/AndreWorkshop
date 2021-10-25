using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencySlider : MonoBehaviour
{
    [Range(0, 100)]
    public float wantedFreq;

    [SerializeField]
    [Range(0, 100)]
    private float frequency;

    [SerializeField]
    private Material dimSeeMaterial;

    [SerializeField]
    private float xLimit = 120;

    [SerializeField]
    float rotSpeed = 50;

    float angleX = 0.0f;
   
    
   
    private void Update()
    {
        //this.transform

        //transform.localEulerAngles = new Vector3(frequency * xLimit, 0, 0);

        if(frequency <= wantedFreq + 5 && frequency >= wantedFreq - 5)
        {
            float newStaticLenght = Mathf.Lerp(dimSeeMaterial.GetFloat("StaticLenght"), 0, 0.2f);

            dimSeeMaterial.SetFloat("StaticLenght", newStaticLenght);
        }

    }

    private void OnMouseDrag()
    {
        angleX += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
        angleX = Mathf.Clamp(angleX, -25, xLimit);
        transform.localRotation = Quaternion.Euler(angleX, 0.0f, 0.0f);

        //angleY += Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
        //angleY = Mathf.Clamp(angleY, -20.0f, 20.0f);
        //transform.rotation = Quaternion.Euler(angleY, 0.0f, 0.0f);
    }
}
