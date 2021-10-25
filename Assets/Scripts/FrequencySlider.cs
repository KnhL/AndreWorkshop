using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrequencySlider : MonoBehaviour
{
    public bool selected;

    [Range(0, 100)]
    public float wantedFreq;

    [SerializeField]
    [Range(0, 100)]
    private float frequency;

    [SerializeField]
    private Material dimSeeMaterial;

    [SerializeField]
    private CameraBehavior camBehavior;

    [SerializeField]
    private float xLimit = 120;

    [SerializeField]
    float rotSpeed = 50;

    float angleX = 0.0f;
   
    private void Update()
    {
        frequency = (angleX / xLimit) * 100;

        //transform.localEulerAngles = new Vector3(frequency * xLimit, 0, 0);

        if(frequency <= wantedFreq + 5 && frequency >= wantedFreq - 5)
        {
            float newStaticLenght = Mathf.Lerp(dimSeeMaterial.GetFloat("StaticLenght"), 0, 0.2f);

            dimSeeMaterial.SetFloat("StaticLenght", newStaticLenght);
        }
        else
        {
            float newStaticLenght = Mathf.Lerp(dimSeeMaterial.GetFloat("StaticLenght"), 1, 0.2f);

            dimSeeMaterial.SetFloat("StaticLenght", newStaticLenght);
        }

        if(dimSeeMaterial.GetFloat("StaticLenght") <= 0.01)
        {
            dimSeeMaterial.SetFloat("StaticLenght", 0);
        }
        if(dimSeeMaterial.GetFloat("StaticLenght") >= 0.98)
        {
            dimSeeMaterial.SetFloat("StaticLenght", 1);
        }

        

    }

    private void OnMouseDrag()
    {
        if(selected == true)
        {
            angleX += Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
            angleX = Mathf.Clamp(angleX, 0, xLimit);
            transform.localRotation = Quaternion.Euler(angleX, 0.0f, 0.0f);

            camBehavior.LockCur(true);

            camBehavior.lockCamera = true;
        }
    }

    private void OnMouseUp()
    {
        if (selected == true)
        {
            camBehavior.LockCur(false);

            camBehavior.lockCamera = false;
        }

        if (selected == false)
        {
            camBehavior.LockCur(true);

            camBehavior.lockCamera = false;
        }
    }
}
