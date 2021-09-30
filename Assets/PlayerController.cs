using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [Header("Control values")]
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float maxGroundVelocity, maxJumpVelocity;
    [Header("Current Values")]
    [SerializeField] private Vector3 currentVelocity;
    [SerializeField] private bool onGround = true;

    private void Start()
    {
        //Sets rigidbody
        rb = transform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        #region Velocity Change
        //Sets velocity
        Vector3 MoveVelocity = rb.velocity + Vector3.Lerp(rb.velocity, (transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")), speed * Time.fixedDeltaTime);

        //Clamping Velocity
        MoveVelocity.x = Mathf.Clamp(MoveVelocity.x, -maxGroundVelocity, maxGroundVelocity);
        MoveVelocity.y = Mathf.Clamp(MoveVelocity.y, -maxGroundVelocity, maxGroundVelocity);
        MoveVelocity.z = Mathf.Clamp(MoveVelocity.z, -maxGroundVelocity, maxGroundVelocity);

        //Moves rigidbody in the facing direction
        rb.velocity = MoveVelocity;
        #endregion
    }
}
