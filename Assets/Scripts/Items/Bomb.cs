using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Bomb : Item
{


    public GameObject explosion;
    public float force;

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
        base.Update();
        transform.Rotate(Vector3.up, .10f);

        if (lifeTime - currentTime <= 0)
        {
            PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (GameManager.Instance.finishRace) return;

        if (other.GetComponent<CarModel>())
        {
            other.GetComponent<CarModel>().Stuned = true;
            //PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }


        if (other.gameObject.GetComponent<Rocket>())
        {
            PhotonNetwork.Instantiate(explosion.name, transform.position, Quaternion.identity);
            PhotonNetwork.Destroy(gameObject);
        }

        if (other.gameObject.GetComponent<Field>())
        {
            PhotonNetwork.Destroy(gameObject);
        }

    }

}
