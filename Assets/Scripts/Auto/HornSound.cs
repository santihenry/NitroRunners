using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class HornSound : MonoBehaviourPun,IObserver
{
    public List<AudioClip> sounds;
    private AudioSource source;
    private CarModel _carModel;

    private void Awake()
    {
        _carModel = GetComponentInParent<CarModel>();
        source = GetComponent<AudioSource>();
        var obs = FindObjectOfType<CarModel>().GetComponent<IObservable>();
        if (obs != null) obs.SubEvent(this);
    }



    [PunRPC]
    void Horn()
    {
        source.clip = sounds[Sounds.horn];
        source.Play();
    }

    public void OnNotify(string eventName)
    {
        if(eventName == "Horn")
        {
            photonView.RPC("Horn", RpcTarget.All);
        }
    }
}
