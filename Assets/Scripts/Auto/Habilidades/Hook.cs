using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Hook : MonoBehaviourPun
{
    [SerializeField] float hookForce = 50f;

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
    private void Start()
    {
        
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
        var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
        transform.position = new Vector3(transform.position.x, hitMedio.point.y + 1, transform.position.z);
        rb.AddForce(transform.forward * hookForce, ForceMode.Impulse);
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
    }
}
