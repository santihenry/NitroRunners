using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;


public class SecuencialLandMines : MonoBehaviourPun
{
    float currentTime;
    List<Item> listBullets = new List<Item>();
    Queue<Item> bulletSatck = new Queue<Item>();
    bool canDropGranade = true;
    public float fireRate;
    public GameObject bomb;
    public Transform origin;
    CarModel _car;


    private void Start()
    {
        _car = GetComponentInParent<CarModel>();
    }


    bool shoot;

    void Update()
    {
        if (!photonView.IsMine) return;

        currentTime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.G) && bulletSatck.Any())
        {
            
            StartCoroutine(Sequence(bulletSatck));
            canDropGranade = false;
        }

    }

    IEnumerator Sequence(Queue<Item> l)
    {
        while (bulletSatck.Any())
        {
            var c4 = bulletSatck.Dequeue().GetComponent<SecuencialBoom>();
            c4.Explotion();
            yield return new WaitForSeconds(.2f);
        }
        canDropGranade = true;
        shoot = false;
    }

   
    public void Trigger()
    {
        bomb.GetComponent<Animator>().SetTrigger("Activate");
    }

    public void Shoot()
    {
        if (fireRate - currentTime <= 0 && canDropGranade && bulletSatck.Count < 6)
        {
            //var bullet = PhotonNetwork.Instantiate("BombaSecuencia", GetComponent<CarModel>().sapwnPoint.position, Quaternion.identity).GetComponent<Item>();
            var bullet = PhotonNetwork.Instantiate("BombaSecuencia",origin.position, origin.rotation).GetComponent<Item>();
            listBullets.Add(bullet);
            bulletSatck.Enqueue(bullet);
            currentTime = 0;

            if (!shoot)
            {
                _car.DeleyToUlti = 0;
                _car.timeToUlti = 0;
                shoot = true;
            }
        }
    }
}
