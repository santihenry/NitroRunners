using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverBoard : MonoBehaviour
{
    Rigidbody rb;
    public float mult;
    public Transform[] anchors = new Transform[4];
    public RaycastHit[] hits = new RaycastHit[4];

    public float turnTorque;
    public float moveForce;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyF(anchors[i], hits[i]);

        rb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
        //rb.AddForce(Input.GetAxis("Horizontal") * moveForce * transform.right);
        rb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);

    }

    void ApplyF(Transform anchor, RaycastHit hit)
    {
        if (Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            rb.AddForceAtPosition(transform.up * force * mult, anchor.position, ForceMode.Acceleration);
        }
    }


}
