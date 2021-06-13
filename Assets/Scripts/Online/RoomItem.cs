using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;


public class RoomItem : MonoBehaviour
{

    public TMP_Text roomInfoTxt;

    public RoomInfo roomInfo;

    public RoomInfo RoomInfo
    {
        get
        {
            return roomInfo;
        }
        private set
        {
            roomInfo = value;
        }
    }

    private void Update()
    {

        GetComponent<Button>().interactable = NetManager.Instance.username.text == "" ? false : true;
    }


    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomInfo = roomInfo;
        roomInfoTxt.text = $"{roomInfo.Name}              {roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";
    }


    public void Join()
    {
        NetManager.Instance.ChangeNickName();
        
        PhotonNetwork.JoinRoom(RoomInfo.Name);
        if (NetManager.Instance.debugInfo != null) NetManager.Instance.debugInfo.text = $"\n join room \n max players {byte.Parse(NetManager.Instance.cantPlayerDropDown.captionText.text)}";
    }


}
