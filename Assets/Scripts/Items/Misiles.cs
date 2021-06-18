using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Misiles : MonoBehaviourPun
{
    public float speedIncreaceSize;
    public float maxSize;
    float size;
    public LayerMask floorLayer;
    public LayerMask carLayer;
    RaycastHit hitMedio;
    float _time;
    Rigidbody _rb;

    bool crashFloor;
    Vector3 _initPos;
    public GameObject Preview;
    Renderer _renderer;

    int _id;


    public Misiles SetId(int id)
    {
        _id = id;
        return this;
    }


    public Misiles SetInitPos(Vector3 pos)
    {
        _initPos = pos;
        return this;
    }


    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        size = 1;
      //  transform.localScale = new Vector3(size, size, size);
        _renderer = Preview.GetComponent<Renderer>();
        _renderer.material.shader = Shader.Find("preview");
        //transform.position = _initPos;
        //Preview.transform.localScale = new Vector3(maxSize/6, maxSize/6, maxSize/6);
        photonView.RPC("SetInitPosRPC", RpcTarget.All,_initPos);
        photonView.RPC("SetIdRPC", RpcTarget.All, _id);
    }

    [PunRPC]
    public void SetIdRPC(int id)
    {
        _id = id;
    }


    [PunRPC]
    public void SetInitPosRPC(Vector3 pos)
    {
        _initPos = pos;
    }

   
    void Update()
    {
        //var backWl = Physics.Raycast(_initPos, Vector3.down, out hitMedio, Mathf.Infinity, floorLayer);
        //if (crashFloor)
        //{
        //    Preview.SetActive(false);

        //    if (size < maxSize)
        //    {
        //        size += speedIncreaceSize * Time.deltaTime;
        //    }
        //    else
        //    {
        //        _time += Time.deltaTime;
        //        foreach (var car in Physics.OverlapSphere(transform.position, maxSize/2, carLayer))
        //        {
        //            if(car.GetComponent<CarModel>().ID != _id)
        //                car.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
        //        }
        //        if(_time>2)
        //            PhotonNetwork.Destroy(gameObject);
        //    }

        //    transform.localScale = new Vector3(size, size, size);
        //    transform.position = hitMedio.point;
        //}
        //else
        //{
        //    Preview.transform.position = hitMedio.point + transform.up * 0.3f;
        //    Preview.transform.up = hitMedio.normal;
        //}
            _renderer.material.SetVector("_previewPosition", Preview.transform.position);
            _renderer.material.SetVector("_position", transform.position);



        if (Vector3.Distance(hitMedio.point, transform.position) < 1) crashFloor = true;

    }

  

    private void FixedUpdate()
    {
        //if (!crashFloor)
        //    _rb.AddForce(Vector3.down);

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10)
        {
            crashFloor = true;
        }
    }
    
}
