using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Grap : MonoBehaviourPun
{
    [SerializeField] float pullSpeed = 5;//0.5f;
    [SerializeField] float stopDistance = 4f;
    [SerializeField] Transform shootTransform;
    public Animator animGears;

    public string hookPath;
    public Vector3 myPosition { get => shootTransform.position; }
    Hook hook;
    bool pulling;
    public Rigidbody _rb;

    bool canShoot;

    public float timeCountdown;
    float currentTime;

    CarModel _car;

    private void Start()
    {
       
        pulling = false;
        _car = GetComponentInParent<CarModel>();
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if(timeCountdown - currentTime <= 0)
        {
            canShoot = true;           
        }
       
        if (!pulling || hook == null) return;

        if (Vector3.Distance(transform.position, hook.transform.position) <= stopDistance)
        {
            DestroyHook();
        }
        else
        {
            _rb.AddForce((hook.transform.position - transform.position).normalized * pullSpeed, ForceMode.VelocityChange);
        }
    }


    public void Shoot()
    {
        if (!canShoot) return;
        currentTime = 0;
        canShoot = false;
        StopAllCoroutines();
        pulling = false;
        hook = PhotonNetwork.Instantiate(hookPath, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
        hook.Initialized(this, /*shootTransform*/_rb.transform);
        StartCoroutine(DestroyHookAfterLifeTime());
    }


    public void StartPull()
    {
        pulling = true;
        if (_car != null)
        {
            _car.Ulti = false;
            _car.DeleyToUlti = 0;
            _car.timeToUlti = 0;
        }
    }

    private void DestroyHook()
    {
        if (hook == null) return;

        pulling = false;
        hook.Destroy();
        hook = null;
    }
    private IEnumerator DestroyHookAfterLifeTime()
    {
        yield return new WaitForSeconds(1f);        
        DestroyHook();
    }
    public void GearsUp()
    {
        animGears.SetBool("build up",true);
    }
    public void GearsDown()
    {
        animGears.SetBool("build up", false);
    }
   
}






























/*
       if (hook == null && Input.GetMouseButtonDown(0))
       {
           StopAllCoroutines();
           pulling = false;
           hook = PhotonNetwork.Instantiate(hookPath, shootTransform.position, Quaternion.identity).GetComponent<Hook>();
           hook.Initialized(this, shootTransform);
           StartCoroutine(DestroyHookAfterLifeTime());
       }
       else if (hook !=null && Input.GetMouseButtonDown(1))
       {
           DestroyHook();
       }
       */
