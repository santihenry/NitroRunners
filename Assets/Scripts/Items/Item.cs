using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Item : MonoBehaviourPun
{

    public float lifeTime;
    protected float currentTime;
    public List<AudioClip> sounds;
    protected AudioSource source;
    protected CarModel car;
    protected int _id;
    public string path;


    public Vector3 _dir;
    public Transform tirador;


    public  int ID
    {
        get
        {
            return _id;
        }
        set
        {
            _id = value;
        }
    }

    [PunRPC]
    public void ChangeId(int id)
    {
        _id = id;
        Debug.LogWarning($"id     : ({_id})");
    }

    [PunRPC]
    public void SetCar(CarModel c)
    {
        car = c;
    }


    public virtual void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    
    public virtual void Start()
    {
        
        foreach (var item in FindObjectsOfType<CarModel>())
        {
            if (item.photonView.ViewID == ID)
            {
                car = item;
            }
        }
        
    }
   

    public virtual void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > lifeTime)
           PhotonNetwork.Destroy(gameObject);
    }
}
