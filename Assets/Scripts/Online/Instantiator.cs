using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Instantiator : MonoBehaviourPun
{
    public Transform[] spawnPoints;
    public List<GameObject> cars = new List<GameObject>();

    string prefabPosition = "PlayerPosMap";
    public List<GameObject> carsPos = new List<GameObject>();

    public static Instantiator Instance { get; set; }


    void Start()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PhotonNetwork.NickName))
        {
            var car = PhotonNetwork.Instantiate(cars[(int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.NickName]].name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
            var x = PhotonNetwork.Instantiate(prefabPosition, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
            x.GetComponent<PlayerPos>().SetTarget = car.transform;            
            x.GetComponent<PlayerPos>().num = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.NickName];         
        }
        else
        {
            var car = PhotonNetwork.Instantiate(cars[0].name, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
            var x = PhotonNetwork.Instantiate(prefabPosition, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].position, spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber].rotation);
            x.GetComponent<PlayerPos>().SetTarget = car.transform;
            x.GetComponent<PlayerPos>().num = (int)PhotonNetwork.CurrentRoom.CustomProperties[PhotonNetwork.NickName];
        }
    }



}
