using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    //SPAWN OBJECTS ----------------------------------------

    //[Header("SpawnVariables")]

    //[SerializeField]
    //private GameObject newBottle;

    //GRAB OBJECT ------------------------------------------

    //[Header("GrabVariables")]

    //[Space(10)]

    //public bool objGrabbed = false;

    //public GameObject grabbedObj;

    //[SerializeField]
    //private Transform objGoToPoint;

    //[SerializeField]
    //private Transform minGrabPoint;

    //[SerializeField]
    //private Transform maxGrabPoint;

    //[SerializeField]
    //private float grabDelay = 2.0f;

    //[SerializeField]
    //private float grabTime;

    //[SerializeField]
    //private float raycastLenght = 5f;

    //[SerializeField]
    //private float throwPower;

    //[SerializeField]
    //private float maxThrowPower = 250;

    //private RaycastHit rHit;



    //CAMERA SMOOTHING --------------------------------------

    [Header("CamVariables")]

    [Space(10)]

    public Vector2 sensitivity = new Vector2(2, 2);

    public bool lockCursor;

    public bool lockCamera;

    public Vector2 clampInDegrees = new Vector2(360, 180);

    public Vector2 targetDirection;

    public Vector2 targetCharacterDirection;

    public GameObject characterBody;

    [SerializeField]
    private Vector2 smoothing = new Vector2(3, 3);

    private Vector2 _mouseAbsolute;

    private Vector2 _smoothMouse;

    // Assign this if there's a parent object controlling motion, such as a Character Controller.
    // Yaw rotation will affect this object instead of the camera if set.

    void Start()
    {
        // Set target direction to the camera's initial orientation.
        targetDirection = transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        if (characterBody)
        {
            targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
        }

        //grabTime = Time.time + grabDelay;
    }

    public void LockCur(bool a)
    {
        lockCursor = a;

        //Debug.Log("LockCur called");
    }

    private void Update()
    {

        if(lockCursor == false)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }


        // Ensure the cursor is always locked when set
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;

            if(lockCamera == false)
            {
                // Allow the script to clamp based on a desired target value.
                var targetOrientation = Quaternion.Euler(targetDirection);
                var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

                // Get raw mouse input for a cleaner reading on more sensitive mice.
                var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

                // Scale input against the sensitivity setting and multiply that against the smoothing value.
                mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

                // Interpolate mouse movement over time to apply smoothing delta.
                _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
                _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

                // Find the absolute mouse movement value from point zero.
                _mouseAbsolute += _smoothMouse;

                // Clamp and apply the local x value first, so as not to be affected by world transforms.
                if (clampInDegrees.x < 360)
                    _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

                // Then clamp and apply the global y value.
                if (clampInDegrees.y < 360)
                    _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

                transform.localRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right) * targetOrientation;

                // If there's a character body that acts as a parent to the camera
                if (characterBody)
                {
                    var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, Vector3.up);
                    characterBody.transform.localRotation = yRotation * targetCharacterOrientation;
                }
                else
                {
                    var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
                    transform.localRotation *= yRotation;
                }
            }
        }
    }
}
