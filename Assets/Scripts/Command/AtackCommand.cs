using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class AtackCommand : ICommand
{
    Roulete ruleta;

    public void Execute(GameObject obj)
    {
        var car = obj.GetComponent<CarModel>();

        if (!ruleta.CanDrop && car.HaveItem || !car.HaveItem || !ruleta.CanDrop) return;
        car.HaveItem = false;
        car.cantItem = 0;
        ruleta.rouleteImg.gameObject.SetActive(false);
        ruleta.frameRouleteImg.gameObject.SetActive(false);
        //var spawn = car.weapon == car.zapPath ? car.sapwnPointZap : car.sapwnPoint;
        var spawn = car.sapwnPoint;
        var item = PhotonNetwork.Instantiate(car.weapon, spawn.position, spawn.rotation);
        item.GetPhotonView().RPC("ChangeId", RpcTarget.AllBuffered, car.photonView.ViewID);

    }

    public void Stop(GameObject obj)
    {
        var car = obj.GetComponent<CarModel>();
        if (!ruleta.CanDrop && car.HaveItem || !car.HaveItem || !ruleta.CanDrop || car.weapon == "") return;
        car.HaveItem = false;
        car.cantItem = 0;
        car.weapon = "";
    }

    public void Undo(GameObject obj)
    {
        obj.GetComponent<CarModel>().HaveItem = true;
    }


    public void Init(GameObject obj)
    {
        ruleta = obj.GetComponent<Roulete>();
    }
}


   
