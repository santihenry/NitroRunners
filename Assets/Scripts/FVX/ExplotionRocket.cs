using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class ExplotionRocket : MonoBehaviourPun
{
    float lifetime = .5f;
    float currentTime;
   
    void Start()
    {
        
    }

    
    void Update()
    {
        currentTime += Time.deltaTime;
        if(lifetime-currentTime <= 0)
        {
            PhotonNetwork.Destroy(gameObject);
        }
        
    }
}
