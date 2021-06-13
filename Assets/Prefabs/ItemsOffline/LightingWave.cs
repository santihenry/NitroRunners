
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightingWave : MonoBehaviour
{

    public int bounces;//Cuantas veces va a saltar antes de que se destruya.
    public float moveSpeed, rotSpeed, stopDistance, destroyTimer;

    private Transform target, loopTransform;
    public ParticleSystem impact, loop;
    private bool isDead = false;

    private List<Transform> possibleTargets = new List<Transform>();
    private List<Transform> carTargets = new List<Transform>();

    public LayerMask layerMask;
    public float radius;

    public LayerMask floorLayer;
    RaycastHit hitMedio;

    public bool activated;


    private Vector3 _dir;




    public float speed;
    float currentTime;
    public float deleyBoom;

    Transform tirador;

    public LightingWave SetDir(Vector3 dir)
    {
        _dir = dir;
        return this;
    }

    public LightingWave SetMaster(Transform mast)
    {
        tirador = mast;
        return this;
    }


    private void Start()
    {
        loopTransform = loop.transform;

        if (!target && possibleTargets.Count > 0)
        {
            possibleTargets = new List<Transform>();
            target = TakeTarget();
        }

        GetComponent<SphereCollider>().radius = radius;
    }

    private void Update()
    {

        currentTime += Time.deltaTime;

        if (deleyBoom - currentTime < 0 && !activated)
        {
            activated = true;
            impact.Play();
            possibleTargets = new List<Transform>();
            Destroy(gameObject, 3);
        }
        var backWl = Physics.Raycast(transform.position, Vector3.down, out hitMedio, 100, floorLayer);
        if(hitMedio.point!=Vector3.zero)
            transform.position = new Vector3(transform.position.x, hitMedio.point.y + 2, transform.position.z);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            activated = true;
            possibleTargets = new List<Transform>();
        }
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
                if (possibleTargets.Contains(target)) 
                { 
                    possibleTargets.Remove(target);
                }

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
                var random = Random.Range(0, possibleTargets.Count);
                target = TakeTarget();
            }
        }
    }

    void Damage()
    {
        //Modificar la velocidad
        carTargets.Add(target);
        possibleTargets.Remove(target);
        impact.Play();

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
        Destroy(gameObject, destroyTimer);
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


    private void OnTriggerExit(Collider other)
    {
        if (tirador != other.transform && other.gameObject.layer == layerMask && !isDead)
        {
            possibleTargets.Remove(other.transform);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, .5f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

}
