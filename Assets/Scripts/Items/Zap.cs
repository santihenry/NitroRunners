using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;


public class Zap : Item
{

    public int bounces;
    public float moveSpeed, rotSpeed, stopDistance, destroyTimer;

    private Transform target, loopTransform;
    public ParticleSystem impact, loop;
    private bool isDead = false;

    private List<Transform> possibleTargets = new List<Transform>();
    private List<Transform> carTargets = new List<Transform>();

    public LayerMask layerMask;
    public float radius;
    public bool activated;
    public float speed;
    public float deleyBoom;
    public LayerMask floorLayer;
    RaycastHit hitMedio;


    public override void Start()
    {
        base.Start();
        if (!photonView.IsMine) return;

        loopTransform = loop.transform;


        
        if (!target && possibleTargets.Count > 0)
        {
            possibleTargets = new List<Transform>();
            target = TakeTarget();
        }

        GetComponent<SphereCollider>().radius = radius;
       
        _dir = Vector3.forward;

        foreach (var item in FindObjectsOfType<CarModel>())
        {
            if (item.photonView.ViewID == ID)
                tirador = item.transform;
        }

                    
        Debug.LogWarning($"DIR :  {_dir}   |    Car : {tirador}   |  ID :   {_id} ");
    }

    public override void Update()
    {
        base.Update();

        if (!photonView.IsMine) return;

        currentTime += Time.deltaTime;

        if (deleyBoom - currentTime < 0 && !activated)
        {
            activated = true;
            //impact.Play();
            photonView.RPC("PlayImpact", RpcTarget.All);
            possibleTargets = new List<Transform>();
        }

    

         var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
        if(hitMedio.point!=Vector3.zero) 
            transform.position = new Vector3(transform.position.x, hitMedio.point.y + 2, transform.position.z);

         Shoot();
        foreach (var item in Physics.OverlapSphere(transform.position, radius, layerMask))
        {
            if (tirador != item.transform && !possibleTargets.Contains(item.transform) && target != item.transform && !isDead && !carTargets.Contains(item.transform))
                possibleTargets.Add(item.GetComponent<Transform>());
        }
        

        if (!isDead)
        {
            if (target)
            {
                if (possibleTargets.Contains(target)) { possibleTargets.Remove(target); }

                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                transform.LookAt(target);

                loopTransform.Rotate(Vector3.up, rotSpeed * Time.deltaTime, Space.Self);

                var distance = Vector3.Distance(transform.position, target.position);


                if (distance < stopDistance)
                {                  
                    Damage();
                }
            }
            else if (possibleTargets.Count > 0)
            {
                possibleTargets.Remove(target);
                target = TakeTarget();
            }
        }

    }
    [PunRPC]
    public void PlayImpact()
    {
        impact.Play();
    }
    void Damage()
    {
        //Modificar la velocidad
       
        target.gameObject.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
        carTargets.Add(target);
        possibleTargets.Remove(target);
       // impact.Play();
        photonView.RPC("PlayImpact", RpcTarget.All);

        if (possibleTargets.Count > 0)
        {
            target = TakeTarget();
            bounces -= 1;
            if (bounces <= 0)
            {
                FinishWave();
            }
        }
        else
        {
            FinishWave();
        }
    }

    private void FinishWave()
    {
        isDead = true;
        var loopEmi = loop.emission;
        loopEmi.rateOverTime = 0;
        //Destroy(gameObject, destroyTimer);
        //PhotonNetwork.Destroy(gameObject);
    }


    private Transform TakeTarget()
    {
        var targets = SortTargets();
        possibleTargets = new List<Transform>();
        possibleTargets.AddRange(targets);
        return possibleTargets[0];
    }


    private IEnumerable<Transform> SortTargets()
    {

        return possibleTargets.OrderBy(n => Vector3.Distance(transform.position, n.position));
    }



    public void Shoot()
    {
        if (!activated)
            transform.Translate(_dir * speed * Time.deltaTime);
    }



    private void OnTriggerEnter(Collider other)
    {

        if (!photonView.IsMine) return;


        if (other.gameObject.GetComponent<Field>())
        {
            PhotonNetwork.Destroy(gameObject);
        }
       

       
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine) return;

        if (tirador != other.transform && other.gameObject.layer == layerMask && !isDead)
        {
            possibleTargets.Remove(other.transform);
        }
    }


}
