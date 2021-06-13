using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Test : MonoBehaviourPun
{
    public Text texto;
    public Vector3 offset;
    public CarModel car;
    public int _id;

    public Test SetCar(CarModel c)
    {
        car = c;
        return this;
    }

    public int ID
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
    }



    private void Start()
    {
        transform.parent = GameObject.Find("Canvas").gameObject.transform;
        photonView.RPC("Auto", RpcTarget.AllBuffered, car);
    }


    [PunRPC]
    public void Auto(CarModel m)
    {
        car = m;
    }

    private void Update()
    {
        if(car != null)
        {
            if (!photonView.IsMine)
                texto.gameObject.SetActive(car.GetComponent<CarController>().hideNick);
            else
                texto.gameObject.SetActive(false);
        }
       

        //if (!photonView.IsMine) return;
        foreach (var item in FindObjectsOfType<CarModel>())
        {
            if (item.photonView.ViewID == ID)
            {
                car = item;
                photonView.RPC("ChangeNickName", RpcTarget.AllBuffered, car.nickName);
                break;
            }
        }      
        Vector3 pos = car.cam.GetComponent<Camera>().WorldToScreenPoint(car.transform.position + offset);
        transform.position = pos;          
    }

    [PunRPC]
    public void ChangeNickName(string name)
    {
        texto.text = name;
        texto.text.ToUpper();
    }


}
