using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Wall : Item
{
    public ParticleSystem destroyParticle;
    public GameObject wall;
    public Rigidbody rigidbody;
    public Collider collider;

    public GameObject columna1, columna2;


    public override void Awake()
    {
        base.Awake();        
    }

    public override void Start()
    {
        base.Start();
        if (!car.photonView.IsMine)
            source.clip = sounds[Sounds.forceField];
        source.Play();
    }

    public override void Update()
    {
        base.Update();

        if (GameManager.Instance.finishRace)
        {
            gameObject.SetActive(false);
        }

    }

    private void OnParticleSystemStopped()
    {
        PhotonNetwork.Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CarModel>().photonView.ViewID != ID || collision.gameObject.GetComponent<Field>())
        {
            photonView.RPC("Destroy", RpcTarget.All);
        }
    }


    [PunRPC]
    void Destroy()
    {
        destroyParticle.Play();
        wall.gameObject.SetActive(false);
        collider.enabled = false;
        rigidbody.isKinematic = true;
        columna1.SetActive(false);
        columna2.SetActive(false);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rocket>() || other.gameObject.GetComponent<CarModel>().photonView.ViewID != ID)
        {
            photonView.RPC("Destroy", RpcTarget.All);
        }
    }


}
