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

        currentTime += Time.deltaTime;

        if (lifeTime - currentTime <= 0)
        {
            car.Inmortality = false;
            PhotonNetwork.Destroy(gameObject);
        }

        car.Inmortality = true;

        transform.position = car.gameObject.transform.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarModel>() && other.GetComponent<CarModel>().photonView.ViewID != ID)
        {
            other.GetComponent<CarModel>().Stuned = !other.GetComponent<CarModel>().Stuned;
        }
    }


}
