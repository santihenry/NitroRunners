using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class ScanElectric : Item
{
    [Range(.1f,1f)]
    public float speedIncreaceSize;
    public float maxSize;
    float size;
    public LayerMask floorLayer;
    RaycastHit hitMedio;



    public override void Start()
    {
        base.Start();
        size = 0;
    }



    public override void Update()
    {
        if (size < maxSize)
        {
            transform.position = car.transform.position;
            size += speedIncreaceSize;
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
        }

        transform.localScale = new Vector3(size, transform.localScale.y, size);
        
    }

    private void FixedUpdate()
    {
        var x = Physics.Raycast(transform.position, Vector3.down, out hitMedio, .5f, floorLayer) ? true : false;

        transform.rotation = Quaternion.FromToRotation(transform.up, hitMedio.normal) * transform.rotation;
     
    }


    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.finishRace) return;


        if (other.gameObject.GetComponent<Field>())
        {
            PhotonNetwork.Destroy(gameObject);
        }

        if (other.GetComponent<CarModel>() && other.GetComponent<CarModel>().photonView.ViewID != ID)
        {
            other.GetComponent<CarModel>().Stuned = true;
        }
    }

}
