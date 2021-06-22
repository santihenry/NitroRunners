using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Hook : MonoBehaviourPun
{
    [SerializeField] float hookForce = 100f;

    Grap _grap;
    Rigidbody rb;
    LineRenderer lr;
    public Transform hookTransform;
    public LayerMask floorLayer;
    RaycastHit hitMedio;

    public void Initialized(Grap grap,Transform shootTransform)
    {
        transform.forward = shootTransform.forward;
        _grap = grap;
        rb = GetComponent<Rigidbody>();
        lr = GetComponent<LineRenderer>();
        rb.AddForce(transform.forward * hookForce, ForceMode.Impulse);
    }

    private void Update()
    {

        Vector3[] positions = new Vector3[]
        {
            //transform.position,
            hookTransform.position,
            _grap.myPosition
        };
        lr.SetPositions(positions);

        
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * hookForce, ForceMode.Impulse);
        var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
        transform.position = new Vector3(transform.position.x, hitMedio.point.y + 2, transform.position.z);
    }

    public void Destroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<CarModel>())
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            _grap.StartPull();
        }

        if (other.gameObject.layer == 10)
        {
            rb.useGravity = false;
            rb.isKinematic = true;

            _grap.StartPull();
        }
    }

}
