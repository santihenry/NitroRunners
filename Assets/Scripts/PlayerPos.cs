using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class PlayerPos : MonoBehaviourPun
{

    Transform tgt;
    public Sprite[] sprite;
    public Image img;
    public int num;

    public Transform SetTarget
    {
        set
        {
            tgt = value;
        }
    }


    private void Start()
    {
        if (photonView.IsMine)
            photonView.RPC("ChangeSprite", RpcTarget.AllBuffered, num);
               
    }

    void Update()
    {
        if (!photonView.IsMine || tgt == null) return;      
        transform.position = new Vector3(tgt.transform.position.x, tgt.transform.position.y + 350, tgt.transform.position.z);
        if (GameManager.Instance.finishRace)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    [PunRPC]
    public void ChangeSprite( int faceIndex)
    {
        img.sprite = sprite[faceIndex];//tgt.GetComponent<CarModel>().statsData.pjImg;
    }



}
