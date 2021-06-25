using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class Rocket : Item
{
    [SerializeField]
    private GameObject particle;
    private Rigidbody rb;
    private bool _iddle;
    public float moveForce;
    public GameObject explosion;
    int currentWay;
    float horizontal;
    public List<Transform> orderList = new List<Transform>();

    public LayerMask floorLayer;
    RaycastHit hitMedio;
    public GameObject Model;
    public LayerMask carLayer;


    public Vector3 trigerSize;
    public Vector3 trigerOffset;

    bool choque;
    public float radiusExplotion;


    public override void Awake()
    {
        base.Awake();
        particle.SetActive(false);
        rb = GetComponent<Rigidbody>();
        _iddle = true;

    }

    public override void Start()
    {
        base.Start();

        particle.SetActive(true);
        _iddle = false;
        photonView.RPC("ChangeIddleValueRPC", RpcTarget.All, false);
        _dir = car.transform.forward;
    }


    public override void Update()
    {
        if (!_iddle)
            currentTime += Time.deltaTime;

        if (lifeTime - currentTime <= 0)
        {
            PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }

        /*
        if (car == null)
        {
            foreach (var item in FindObjectsOfType<CarModel>())
            {
                if (item.photonView.ViewID == _id)
                {
                    car = item;
                }
            }
        }
        */


        if (Input.GetKeyDown(KeyCode.LeftShift) && photonView.IsMine)
        {
            particle.SetActive(true);
            _iddle = false;
            photonView.RPC("ChangeIddleValueRPC", RpcTarget.All, false);

            /*
             if (car.Rocket)
             {
                 source.clip = sounds[Sounds.rocketLaunch];
                 source.Play();
             }
            */

            _dir = car.transform.forward;
        }

        
        if (!_iddle)
        {
            foreach (var car in Physics.OverlapBox(transform.position + trigerOffset, trigerSize, transform.rotation, carLayer))
            {
                if (car.GetComponent<CarModel>().photonView.ViewID != ID)
                {
                    choque = true;
                }
            }
        }

        if (choque)
        {
            deleyDestroy += Time.deltaTime;

            if (deleyDestroy >= .1f)
            {
                PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
                PhotonNetwork.Destroy(gameObject);
            }

            foreach (var car in Physics.OverlapSphere(transform.position, radiusExplotion, carLayer))
            {
                car.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
            }
        }

    }


    float deleyDestroy;

 
    [PunRPC]
    public void ChangeIddleValueRPC(bool value)
    {
        _iddle = value;
    }

    void FixedUpdate()
    {
        //if (!photonView.IsMine) return;

        if (_iddle)
        {
            transform.position = car.transform.position - car.transform.right * 2;
            transform.rotation = car.transform.rotation;
        }
        else
        {          
            Movement();         
        }
    }

    void Movement()
    {
        var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
        transform.position = new Vector3(transform.position.x, hitMedio.point.y + 2, transform.position.z);
        transform.rotation = Quaternion.FromToRotation(transform.up, hitMedio.normal) * transform.rotation;
        rb.velocity = (moveForce * transform.forward);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9 && !_iddle)
        {
            choque = true;
        }

        
        if (collision.gameObject.GetComponent<CarModel>() && !_iddle)
        {
            /* if (collision.gameObject.GetComponent<CarModel>().photonView.ViewID == ID) return;
             PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
             PhotonNetwork.Destroy(gameObject);*/           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Field>())
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (other.gameObject.layer == 9 && !_iddle)
        {
            choque = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red / 3;
        Gizmos.DrawWireSphere(transform.position,radiusExplotion);
    }


}
