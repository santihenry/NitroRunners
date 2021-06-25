using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;

public class Drone : Item
{

    private List<Transform> possibleTargets = new List<Transform>();
    private List<Transform> oldsTargets = new List<Transform>();
    public Transform target;
    public int limit;
    public float speed;
    public float radius;
    public float impactDistance;
    public float destroyDelay;
    public float swaySpeed;
    public float swaySize;
    public bool isFirst;

    public GameObject visuals;
    public GameObject spawnFX;
    public GameObject impactFX;

    public GameObject fbx;
    public GameObject trail;

    Vector3 startPosition;


    float timeBeforeImpact;
    public LayerMask layerMask;

    public LayerMask floorLayer;
    public LayerMask wallLayer;
    RaycastHit hitMedio;

    public override void Start()
    {
        base.Start();
        if (!photonView.IsMine) return;


        foreach (var item in FindObjectsOfType<CarModel>())
        {
            if (item.photonView.ViewID == ID)
                tirador = item.transform;
        }



        if (!isFirst)
        {
            startPosition = transform.position;         
            visuals.SetActive(true);
            spawnFX.SetActive(false);
            fbx.transform.localScale = new Vector3(.2f, .2f, .2f);
        }
        else
        {
            trail.SetActive(false);

            foreach (var item in Physics.OverlapSphere(transform.position, radius, layerMask))
            {
                if (tirador != item.transform && !possibleTargets.Contains(item.transform))
                    possibleTargets.Add(item.GetComponent<Transform>());
            }

            SpawnPulse();
            //PhotonNetwork.Destroy(gameObject);
            
        }

        _dir = car.transform.forward;

    }


    public override void Update()
    {
        //base.Update();

        currentTime += Time.deltaTime;
        if (currentTime > lifeTime)
            PhotonNetwork.Destroy(gameObject);

        if (!photonView.IsMine) return;
        if (GameManager.Instance.finishRace) return;

        if (target)
        {
            transform.LookAt(target);
            startPosition += transform.forward * Time.deltaTime * speed;
            transform.position = startPosition + transform.right * Mathf.Sin(Time.time * swaySpeed) * swaySize;

            var distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < impactDistance)
            {
                if (!target.gameObject.GetComponent<CarModel>().Inmortality)
                {
                    tirador.gameObject.GetComponent<CarModel>().Bosting = true;
                    target.gameObject.GetComponent<CarModel>().photonView.RPC("StunedRPC", RpcTarget.All, true);
                }
                impactFX.SetActive(true);
                visuals.SetActive(false);              
                timeBeforeImpact += Time.deltaTime;
              
                if (timeBeforeImpact >= destroyDelay)
                    PhotonNetwork.Destroy(gameObject);
            }
        }
        if (isFirst)
        {
            foreach (var item in Physics.OverlapSphere(transform.position, radius, layerMask))
            {
                if (tirador != item.transform && !possibleTargets.Contains(item.transform) && !oldsTargets.Contains(item.transform))
                {
                    possibleTargets.Add(item.GetComponent<Transform>());
                    oldsTargets.Add(item.GetComponent<Transform>());
                    SpawnPulse();  
                }
            }
            //transform.Rotate(Vector3.up, 3f);
            //transform.position = car.sapwnPointZap.transform.position + new Vector3(0, 2, 0);
            var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
            transform.position = new Vector3(transform.position.x, hitMedio.point.y + 2, transform.position.z);
            transform.position += _dir * 120 * Time.deltaTime;
            impactFX.SetActive(false);
            spawnFX.SetActive(false);


            if(Physics.Raycast(transform.position, transform.forward, out hitMedio, 20, wallLayer))
            {
                if(Physics.Raycast(transform.position, transform.right, out hitMedio, 20, wallLayer))
                {
                    transform.Rotate(Vector3.up, -10);
                }
                else if (Physics.Raycast(transform.position, -transform.right, out hitMedio, 20, wallLayer))
                {
                    transform.Rotate(Vector3.up, 10);
                }

                _dir = transform.forward;
            }


        }
    }




    private void SpawnPulse()
    {

        for (int i = possibleTargets.Count-1; i >= 0; i--)
        {
            var item = PhotonNetwork.Instantiate("Drone", transform.position, Quaternion.identity);
            item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, ID);
            item.GetComponent<Drone>().isFirst = false;
            item.GetComponent<Drone>().target = TakeTarget();

            if (possibleTargets.Any())
                possibleTargets.RemoveAt(0);

            limit -= 1;
            if (limit <= 0)
            {
                break;
            }
        }
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



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Field>())
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

}
