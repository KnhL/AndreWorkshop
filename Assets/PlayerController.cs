using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, jumpMultiplier;
    private Rigidbody rb;
    private Vector3 moveDir;
    bool onGround;
    public List<GameObject> route;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        moveDir = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            onGround = false;
            rb.AddForce(new Vector3(0, jumpMultiplier, 0));
        }

        #region Debugs
        //Debug lines
        Debug.DrawLine(transform.position, transform.position + transform.forward * 1.5f, Color.blue);
        Debug.DrawLine(transform.position, transform.position + (transform.forward - transform.right) * 1.5f, Color.red);
        Debug.DrawLine(transform.position, transform.position + (transform.forward + transform.right) * 1.5f, Color.red);
        #endregion
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + (moveDir * speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Floor")
        {
            route.Add(collision.gameObject);
            onGround = true;
        }
    }
}
