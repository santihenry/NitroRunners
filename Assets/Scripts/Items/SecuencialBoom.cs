using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class SecuencialBoom : Item
{
    public GameObject explosion;
    public float force;

    public LayerMask floorLayer;
    RaycastHit hitMedio;

    
    public LayerMask targetLayerMask;
    public float radius;

    public GameObject viewMinimap;

    public override void Start()
    {
        base.Start();
        
        if(photonView.IsMine)
            viewMinimap.SetActive(true);
       
    }


    public void Explotion()
    {
        foreach (var item in Physics.OverlapSphere(transform.position, radius, targetLayerMask))
        {
            item.gameObject.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
        }
        PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
        PhotonNetwork.Destroy(gameObject);
    }


}
