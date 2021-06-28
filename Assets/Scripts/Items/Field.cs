using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Field : Item
{
    public override void Awake()
    {
        base.Awake();
    }



    public override void Start()
    {
        base.Start();
       
    }

    public override void Update()
    {
        currentTime += Time.deltaTime;

        if (lifeTime - currentTime <= 0)
        {
            car.Inmortality = false;
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            car.Inmortality = true;
        }


        transform.position = car.gameObject.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarModel>())
        {
            if(other.GetComponent<CarModel>().photonView.ViewID != ID)
                other.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
        }
    }


}
